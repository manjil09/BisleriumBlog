using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBlog(BlogDTO blog)
        {
            var data = await blogRepository.AddBlog(blog);
            return Ok(data);
        }

        //for get blog without authentication
        //[AllowAnonymous]
    }
}
