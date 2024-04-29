using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public async Task<IActionResult> AddBlog(BlogDTO blog)
        {
            var data = await blogRepository.AddBlog(blog);
            var response = new Response<BlogDTO> { IsSuccess = true, Message = "Blog creation successful.", Result = data };
            return Ok(response);
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> GetBlogById(int blogId)
        {
            try
            {
                var blog = await blogRepository.GetBlogById(blogId);
                return Ok(new Response<BlogDTO> { IsSuccess = true, Message = "Blog fetch successful.", Result = blog });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new Response<List<BlogDTO>> { IsSuccess = false, Message = ex.Message });
            }
        }
    }
}
