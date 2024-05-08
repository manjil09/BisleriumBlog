using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.DTOs.CommentReactionDTO
{
    public class CommentReactionResponseDTO
    {
        public int TotalUpvotes { get; set; }
        public int TotalDownvotes { get; set; }
        public string CommentCreatorId { get; set; }
        public ReactionType Type { get; set; }
    }
}
