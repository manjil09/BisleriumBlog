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
        private readonly IBlogRepository blogRepository;
        public BlogController(IBlogRepository blogRepository)
        {
            this.blogRepository = blogRepository;
        }

        //[Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddBlog(BlogDTO blog)
        {
            var data = await blogRepository.AddBlog(blog);
            return Ok(data);
        }


    }
}
