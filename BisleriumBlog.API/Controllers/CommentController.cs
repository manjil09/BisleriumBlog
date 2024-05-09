using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.CommentDTO;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
            try
            {
                var data = await _commentRepository.AddComment(comment);
                var response = new Response<CommentResponseDTO> { IsSuccess = true, Message = "Comment added successfully.", Result = data };
                return Ok(response);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }

        [HttpGet("getComments/{blogId}")]
        public async Task<IActionResult> GetCommentsByBlogId(int blogId, int? pageIndex, int? pageSize)
        {
            if (pageIndex < 1 || pageSize < 1)
                return BadRequest(new Response<string>
                {
                    IsSuccess = false,
                    Message = "Page index and page size must be greater than 0."
                });
            try
            {
                var (totalPages, comments) = await _commentRepository.GetCommentsByBlogId(blogId, pageIndex, pageSize);

                if (comments == null)
                    return NotFound(new Response<string>
                    {
                        IsSuccess = false,
                        Message = "Couldn't fetch any comments."
                    });
                if (totalPages == 0)
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

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("update/{commentId}")]
        public async Task<IActionResult> UpdateComment(int commentId, CommentUpdateDTO updatedComment)
        {
            try
            {
                var result = await _commentRepository.UpdateComment(commentId, updatedComment);
                return Ok(new Response<CommentResponseDTO> { IsSuccess = true, Message = "Comment updated succesfully.", Result = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [Authorize(Roles = "User")]
        [HttpDelete("delete/{commentId}")]
        public async Task<IActionResult> DeleteBlog(int commentId)
        {
            try
            {
                var success = await _commentRepository.DeleteComment(commentId);
                if (success)
                    return Ok(new Response<bool> { IsSuccess = true, Message = "Comment deleted succesfully." });

                return NotFound(new Response<string> { IsSuccess = false, Message = $"Could not find Comment with the id {commentId}" });
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
        }
    }
}
