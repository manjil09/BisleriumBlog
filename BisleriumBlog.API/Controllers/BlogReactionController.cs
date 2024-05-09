using BisleriumBlog.API.SignalRHub;
using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.BlogDTO;
using BisleriumBlog.Application.DTOs.BlogReactionDTO;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Reflection.Metadata;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/blog/reaction")]
    [ApiController]
    public class BlogReactionController : ControllerBase
    {
        private readonly IBlogReactionRepository _blogReactionRepository;
        private IHubContext<NotificationHub, INotificationHub> _hubContext;
        public BlogReactionController(IBlogReactionRepository blogReactionRepository, IHubContext<NotificationHub,INotificationHub> hubContext)
        {
            _blogReactionRepository = blogReactionRepository;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "User")]
        [HttpPost("upvote")]
        public async Task<IActionResult> ToggleUpvote(int blogId, string userId)
        {
            try
            {
                var data = await _blogReactionRepository.ToggleUpvote(blogId, userId);
                string creatorId = data.BlogCreatorId;

                if (creatorId != null)
                {
                    //await _hubContext.Clients.User(creatorId).SendAsync("You received an upvote in your blog.");
                }
                return Ok(new Response<BlogReactionResponseDTO> { IsSuccess = true, Message = "Your reaction to the blog has been updated.", Result = data });
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("downvote")]
        public async Task<IActionResult> ToggleDownvote(int blogId, string userId)
        {
            try
            {
                var data = await _blogReactionRepository.ToggleDownvote(blogId, userId);

                if (data.BlogCreatorId != null)
                {
                    //await _hubContext.send("ReceiveNotification", $"Your blog (ID: {blogId}) has been downvoted.");
                }

                return Ok(new Response<BlogReactionResponseDTO> { IsSuccess = true, Message = "Your reaction to the blog has been updated.", Result = data });
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
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
                    Message = "Blog Reactions retrieved successfully.",
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
                return Ok(new Response<BlogReactionDTO> { IsSuccess = true, Message = "User reaction on the blog fetched successfully.", Result = userReaction });
            }
            catch (Exception ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}
