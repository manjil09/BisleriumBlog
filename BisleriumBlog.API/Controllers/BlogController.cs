using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.BlogDTO;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BisleriumBlog.API.Controllers
{
    [Route("api/blog")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BlogController(IBlogRepository blogRepository, IWebHostEnvironment webHostEnvironment)
        {
            _blogRepository = blogRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "User")]
        [HttpPost("add")]
        public async Task<IActionResult> AddBlog(BlogCreateDTO blog)
        {
            try
            {
                var imageFile = blog.Image;
                ValidateImageFile(imageFile);

                string fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Blog", fileName);
                string imageUrl = Path.Combine("/Images/Blog/", fileName);

                var data = await _blogRepository.AddBlog(blog, imageUrl);
                var response = new Response<BlogResponseDTO> { IsSuccess = true, Message = "Your blog has been posted.", Result = data };

                if (response.IsSuccess)
                    await UploadImage(imageFile, path);

                return Ok(response);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message });
            }
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

            if (totalPages == 0)
                return Ok(new Response<string>
                {
                    IsSuccess = true,
                    Message = "There are no blogs posted yet."
                });

            var response = new Response<dynamic>
            {
                IsSuccess = true,
                Message = "Blogs retrieved successfully.",
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
        public async Task<IActionResult> UpdateBlog(int blogId, BlogUpdateDTO updatedBlog)
        {
            try
            {
                var imageFile = updatedBlog.Image;
                ValidateImageFile(imageFile);

                string fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images/Blog", fileName);
                string imageUrl = Path.Combine("/Images/Blog/", fileName);

                var data = await _blogRepository.UpdateBlog(blogId, updatedBlog, imageUrl);
                var response = new Response<BlogResponseDTO> { IsSuccess = true, Message = "Blog updated succesfully.", Result = data };
                if (response.IsSuccess)
                    await UploadImage(imageFile, path);

                return Ok(response);
            }
            catch (Exception ex)
            {
                string message = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return BadRequest(new Response<string> { IsSuccess = false, Message = message+"jfjfjfj" });
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

        private async Task UploadImage(IFormFile imageFile, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
        }

        private void ValidateImageFile(IFormFile imageFile)
        {
            if (imageFile == null)
                throw new Exception("Please upload an image file.");

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg" };
            string extension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid file type. Please upload an image.");

            if (imageFile.Length > 3 * 1024 * 1024)
                throw new Exception($"The uploaded file exceeds the maximum size limit of 3 MB. Please upload a smaller file.");
        }
    }
}
