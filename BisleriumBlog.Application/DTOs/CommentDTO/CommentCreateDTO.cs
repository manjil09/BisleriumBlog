using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Application.DTOs.CommentDTO
{
    public class CommentCreateDTO
    {
        [Required]
        public string Body { get; set; }
        [Required]
        public int BlogId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
