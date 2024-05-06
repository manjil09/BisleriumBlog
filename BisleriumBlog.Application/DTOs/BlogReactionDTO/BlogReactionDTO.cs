using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.DTOs.BlogReactionDTO
{
    public class BlogReactionDTO
    {
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public ReactionType Type { get; set; }
    }
}
