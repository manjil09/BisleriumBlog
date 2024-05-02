using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [Authorize(Roles = "User")]
        [HttpPost("add")]
        public async Task<IActionResult> AddComment(CommentCreateDTO comment)
        {
            var data = await _commentRepository.AddComment(comment);
            var response = new Response<CommentResponseDTO> { IsSuccess = true, Message = "Comment added successfully.", Result = data };
            return Ok(response);
        }

        [HttpGet("getBlogComments")]
        public async Task<IActionResult> GetCommentsByBlogId(int blogId, int? pageIndex, int? pageSize)
        {
            if (pageIndex < 1 || pageSize < 1)
                return BadRequest(new Response<string>
                {
                    IsSuccess = false,
                    Message = "Page index and page size must be greater than 0."
                });

            var (totalPages, comments) = await _commentRepository.GetCommentsByBlogId(blogId, pageIndex, pageSize);

            if (comments == null)
                return NotFound(new Response<string>
                {
                    IsSuccess = false,
                    Message = "Couldn't fetch any comments."
                });
            if (totalPages == 0 )
                return Ok(new Response<string>
                {
                    IsSuccess = true,
                    Message = "There are currently no comments on this blog."
                });

            var response = new Response<dynamic>
            {
                IsSuccess = true,
                Message = "Comments retrieved successfully.",
                Result = new { TotalPages = totalPages, Comments = comments }
            };

            Response.Headers.Append("X-Total-Pages", totalPages.ToString());

            return Ok(response);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetBlogsByUserId(string userId, int blogId)
        {
            try
            {
                var userComent = await _commentRepository.GetCommentByUserIdAndBlogId(userId,blogId);
                return Ok(new Response<CommentResponseDTO> { IsSuccess = true, Message = "Uer comment o the blog fetched successfully.", Result = userComent });
            }
            catch (Exception ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}
