using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        [Authorize(Roles = "User")]
        [HttpPost("add")]
        public async Task<IActionResult> AddBlog(BlogDTO blog)
        {
            var data = await blogRepository.AddBlog(blog);
            var response = new Response<BlogDTO> { IsSuccess = true, Message = "Blog creation successful.", Result = data };
            return Ok(response);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllBlogs(int? pageIndex, int? pageSize, string sortBy = "random")
        {
            var sortByEnum = Enum.TryParse(sortBy, true, out SortType sortByValue) ? sortByValue : SortType.Recency;

            if (pageIndex < 1 || pageSize < 1)
                return BadRequest(new Response<string>
                {
                    IsSuccess = false,
                    Message = "Page index and page size must be greater than 0."
                });

            var blogs = await blogRepository.GetAllBlogs(pageIndex, pageSize, sortByEnum);

            if (blogs == null)
                return NotFound(new Response<string>
                {
                    IsSuccess = false,
                    Message = "Couldn't fetch any Blogs."
                });

            return Ok(new Response<List<BlogDTO>>
            {
                IsSuccess = true,
                Message = "Blog creation successful.",
                Result = blogs
            });
        }

        [HttpGet("getById/{blogId}")]
        public async Task<IActionResult> GetBlogById(int blogId)
        {
            try
            {
                var blog = await blogRepository.GetBlogById(blogId);
                return Ok(new Response<BlogDTO> { IsSuccess = true, Message = "Blog fetch successful.", Result = blog });
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
                var userBlogs = await blogRepository.GetBlogsByUserId(userId);
                return Ok(new Response<List<BlogDTO>> { IsSuccess = true, Message = "Blog fetch for the user successful.", Result = userBlogs });
            }
            catch(Exception ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPut("update/{blogId}")]
        public async Task<IActionResult> UpdateBlog(int blogId, BlogDTO updatedBlog)
        {
            try
            {
                var result = await blogRepository.UpdateBlog(blogId, updatedBlog);
                return Ok(new Response<BlogDTO> { IsSuccess = true, Message = "Blog updated succesfully.", Result = result });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<string> { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpDelete("delete/{blogId}")]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            var success = await blogRepository.DeleteBlog(blogId);
            if (success)
                return Ok(new Response<bool> { IsSuccess = true, Message = "Blog deleted succesfully." });

            return NotFound(new Response<string> { IsSuccess = false, Message = $"Could not find Blog with the id {blogId}" });
        }
    }
}
