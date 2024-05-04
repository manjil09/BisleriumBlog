using Microsoft.AspNetCore.Http;

namespace BisleriumBlog.Application.DTOs
{
    public class BlogCreateDTO
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public IFormFile Image { get; set; }
        public string UserId { get; set; }
    }
}
