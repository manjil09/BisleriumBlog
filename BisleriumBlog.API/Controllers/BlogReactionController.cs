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

        //[Authorize(Roles = "User")]
        [HttpPost("add")]
        public async Task<IActionResult> AddBlog(BlogReactionDTO reaction)
        {
            try
            {
                var data = await _blogReactionRepository.AddReaction(reaction);
                var response = new Response<BlogReactionDTO> { IsSuccess = true, Message = "Your blog has been posted.", Result = data };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpDelete("delete/{blogId}")]
        public async Task<IActionResult> DeleteBlog(int blogReactionId)
        {
            var success = await _blogReactionRepository.DeleteReaction(blogReactionId);
            if (success)
                return Ok(new Response<bool> { IsSuccess = true, Message = "Reaction removed succesfully." });

            return NotFound(new Response<string> { IsSuccess = false, Message = $"Could not find BlogReaction with the id {blogReactionId}" });
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
    }
}
