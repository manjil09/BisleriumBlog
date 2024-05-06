using BisleriumBlog.Application.DTOs.CommentDTO;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface ICommentRepository
    {
        Task<CommentResponseDTO> AddComment(CommentCreateDTO comment);
        Task<CommentResponseDTO> UpdateComment(int commentId, CommentCreateDTO updatedComment);
        Task<(int, List<CommentResponseDTO>)> GetCommentsByBlogId(int blogId, int? pageIndex, int? pageSize);
        Task<CommentResponseDTO> GetCommentById(int commentId);
        Task<CommentResponseDTO> GetCommentByUserIdAndBlogId(string userId, int blogId);
        Task<bool> DeleteComment(int commentId);
    }
}
