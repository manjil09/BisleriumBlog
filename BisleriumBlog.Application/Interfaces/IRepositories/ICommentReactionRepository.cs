using BisleriumBlog.Application.DTOs;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface ICommentReactionRepository
    {
        Task<CommentReactionDTO> AddCommentReaction(CommentReactionDTO reaction);
        Task<CommentReactionDTO> ToggleCommentReaction(int reactionId, CommentReactionDTO reactionForUpdate);
        Task<List<CommentReactionDTO>> GetCommentReactionByCommentId(int commentId);
        Task<List<CommentReactionDTO>> GetCommentReactionByUserId(string userId);
        Task<bool> DeleteCommentReaction(int blogReactionId);
    }
}
