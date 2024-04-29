using BisleriumBlog.Application.DTOs;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface ICommentRepository
    {
        Task<CommentDTO> AddComment(CommentDTO comment);
        Task<CommentDTO> UpdateComment(int commentId, CommentDTO blogForUpdate);
        Task<List<CommentDTO>> GetCommentsByBlogId(int blogId);
        Task<CommentDTO> GetCommentById(int commentId);
        Task<List<CommentDTO>> GetCommentsByUserId(string userId);
        Task<bool> DeleteComment(int commentId);
    }
}
