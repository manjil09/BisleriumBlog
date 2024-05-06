using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.CommentReactionDTO;
using BisleriumBlog.Application.Interfaces.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/comment/reaction")]
    [ApiController]
    public class CommentReactionController : ControllerBase
    {
        private readonly ICommentReactionRepository _commentReactionRepository;
        public CommentReactionController(ICommentReactionRepository commentReactionRepository)
        {
            _commentReactionRepository = commentReactionRepository;
        }

        [Authorize(Roles = "User")]
        [HttpPost("upvote")]
        public async Task<IActionResult> ToggleUpvote(int commentId, string userId)
        {
            try
            {
                var data = await _commentReactionRepository.ToggleUpvote(commentId, userId);
                var response = new Response<string> { IsSuccess = true, Message = "Your reaction to the comment has been updated." };
                return Ok(response);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("downvote")]
        public async Task<IActionResult> ToggleDownvote(int commentId, string userId)
        {
            try
            {
                var data = await _commentReactionRepository.ToggleDownvote(commentId, userId);
                var response = new Response<string> { IsSuccess = true, Message = "Your reaction to the comment has been updated." };

                return Ok(response);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetReactionById(int id)
        {
            try
            {
                var reaction = await _commentReactionRepository.GetReactionById(id);
                return Ok(new Response<CommentReactionDTO> { IsSuccess = true, Message = "Comment Reaction fetch successful.", Result = reaction });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("getReactions/{commentId}")]
        public async Task<IActionResult> GetReactionsByBlogId(int commentId)
        {
            try
            {
                var (totalUpvotes, totalDownvotes, reactions) = await _commentReactionRepository.GetReactionsByCommentId(commentId);

                if (totalUpvotes == 0 && totalDownvotes == 0)
                    return Ok(new Response<string>
                    {
                        IsSuccess = true,
                        Message = "This comment has not received any reactions yet."
                    });

                var response = new Response<dynamic>
                {
                    IsSuccess = true,
                    Message = "Comment Reactions retrieved successfully.",
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
        [HttpGet("getUserReactions/{commentId}/{userId}")]
        public async Task<IActionResult> GetReactionByUserIdAndBlogId(string userId, int commentId)
        {
            try
            {
                var userReaction = await _commentReactionRepository.GetReactionByUserIdAndCommentId(userId, commentId);
                return Ok(new Response<CommentReactionDTO> { IsSuccess = true, Message = "User reaction on the comment fetched successfully.", Result = userReaction });
            }
            catch (Exception ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}
