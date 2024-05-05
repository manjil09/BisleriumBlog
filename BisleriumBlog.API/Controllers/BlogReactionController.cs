using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/blog/reaction")]
    [ApiController]
    public class BlogReactionController : ControllerBase
    {
        private readonly IBlogReactionRepository _blogReactionRepository;
        public BlogReactionController(IBlogReactionRepository blogReactionRepository)
        {
            _blogReactionRepository = blogReactionRepository;
        }

        [Authorize(Roles = "User")]
        [HttpPost("add")]
        public async Task<IActionResult> AddReaction(BlogReactionDTO reaction)
        {
            try
            {
                var data = await _blogReactionRepository.AddReaction(reaction);
                var response = new Response<BlogReactionDTO> { IsSuccess = true, Message = "Your blog has been posted.", Result = data };
                return Ok(response);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateBlog(int id, BlogReactionDTO updatedReaction)
        {
            try
            {
                var data = await _blogReactionRepository.UpdateReaction(id, updatedReaction);
                var response = new Response<BlogReactionDTO> { IsSuccess = true, Message = "Reaction updated succesfully.", Result = data };

                return Ok(response);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [Authorize(Roles = "User")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReaction(int id)
        {
            var success = await _blogReactionRepository.DeleteReaction(id);
            if (success)
                return Ok(new Response<bool> { IsSuccess = true, Message = "Reaction removed succesfully." });

            return NotFound(new Response<string> { IsSuccess = false, Message = $"Could not find BlogReaction with the id {id}" });
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetReactionById(int id)
        {
            try
            {
                var reaction = await _blogReactionRepository.GetReactionById(id);
                return Ok(new Response<BlogReactionDTO> { IsSuccess = true, Message = "Blog Reaction fetch successful.", Result = reaction });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("getReactions/{blogId}")]
        public async Task<IActionResult> GetReactionsByBlogId(int blogId)
        {
            try
            {
                var (totalUpvotes, totalDownvotes, reactions) = await _blogReactionRepository.GetReactionsByBlogId(blogId);

                if (totalUpvotes == 0 && totalDownvotes == 0)
                    return Ok(new Response<string>
                    {
                        IsSuccess = true,
                        Message = "This blog has not received any reactions yet."
                    });

                var response = new Response<dynamic>
                {
                    IsSuccess = true,
                    Message = "Comments retrieved successfully.",
                    Result = new { TotalUpvotes = totalUpvotes, TotalDownvotes = totalDownvotes, Reactions = reactions }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet("getUserReactions/{blogId}/{userId}")]
        public async Task<IActionResult> GetReactionByUserIdAndBlogId(string userId, int blogId)
        {
            try
            {
                var userReaction = await _blogReactionRepository.GetReactionByUserIdAndBlogId(userId, blogId);
                return Ok(new Response<CommentResponseDTO> { IsSuccess = true, Message = "User reaction on the blog fetched successfully.", Result = userReaction });
            }
            catch (Exception ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}
