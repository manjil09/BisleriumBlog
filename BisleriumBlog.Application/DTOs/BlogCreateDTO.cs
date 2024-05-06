using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Application.DTOs
{
    public class BlogCreateDTO
    {
        [Required]

        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public IFormFile Image { get; set; }
        public string UserId { get; set; }
    }
}
