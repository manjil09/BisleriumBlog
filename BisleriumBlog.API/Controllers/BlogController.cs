using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            this._blogRepository = blogRepository;
        }

        [Authorize(Roles = "User")]
        [HttpPost("add")]
        public async Task<IActionResult> AddBlog(BlogCreateDTO blog)
        {
            var data = await _blogRepository.AddBlog(blog);
            var response = new Response<BlogResponseDTO> { IsSuccess = true, Message = "Blog creation successful.", Result = data };
            return Ok(response);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllBlogs(int? pageIndex, int? pageSize, string sortBy = "recency")
        {
            var sortByEnum = Enum.TryParse(sortBy, true, out SortType sortByValue) ? sortByValue : SortType.Recency;

            if (pageIndex < 1 || pageSize < 1)
                return BadRequest(new Response<string>
                {
                    IsSuccess = false,
                    Message = "Page index and page size must be greater than 0."
                });

            var (totalPages, blogs) = await _blogRepository.GetAllBlogs(pageIndex, pageSize, sortByEnum);

            if (blogs == null)
                return NotFound(new Response<string>
                {
                    IsSuccess = false,
                    Message = "Couldn't fetch any Blogs."
                });

            var response = new Response<dynamic>
            {
                IsSuccess = true,
                Message = "Blog creation successful.",
                Result = new { TotalPages = totalPages, Blogs = blogs }
            };

            Response.Headers.Append("X-Total-Pages", totalPages.ToString());

            return Ok(response);
        }

        [HttpGet("getById/{blogId}")]
        public async Task<IActionResult> GetBlogById(int blogId)
        {
            try
            {
                var blog = await _blogRepository.GetBlogById(blogId);
                return Ok(new Response<BlogResponseDTO> { IsSuccess = true, Message = "Blog fetch successful.", Result = blog });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet("getByUserId/{userId}")]
        public async Task<IActionResult> GetBlogsByUserId(string userId)
        {
            try
            {
                var userBlogs = await _blogRepository.GetBlogsByUserId(userId);
                return Ok(new Response<List<BlogResponseDTO>> { IsSuccess = true, Message = "Blog fetch for the user successful.", Result = userBlogs });
            }
            catch (Exception ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }


        [HttpPut("update/{blogId}")]
        public async Task<IActionResult> UpdateBlog(int blogId, BlogCreateDTO updatedBlog)
        {
            try
            {
                var result = await _blogRepository.UpdateBlog(blogId, updatedBlog);
                return Ok(new Response<BlogResponseDTO> { IsSuccess = true, Message = "Blog updated succesfully.", Result = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpDelete("delete/{blogId}")]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            var success = await _blogRepository.DeleteBlog(blogId);
            if (success)
                return Ok(new Response<bool> { IsSuccess = true, Message = "Blog deleted succesfully." });

            return NotFound(new Response<string> { IsSuccess = false, Message = $"Could not find Blog with the id {blogId}" });
        }
    }
}
