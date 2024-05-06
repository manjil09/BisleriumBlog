using BisleriumBlog.Application.DTOs.CommentReactionDTO;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface ICommentReactionRepository
    {
        Task<bool> ToggleUpvote(int commentId, string userId);
        Task<bool> ToggleDownvote(int commentId, string userId);
        Task<CommentReactionDTO> GetReactionById(int commentReactionId);
        Task<(int totalUpvotes, int totalDownvotes, List<CommentReactionDTO>)> GetReactionsByCommentId(int commentId);
        Task<CommentReactionDTO> GetReactionByUserIdAndCommentId(string userId, int commentId);
    }
}
