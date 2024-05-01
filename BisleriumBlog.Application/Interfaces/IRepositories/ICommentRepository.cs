using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface ICommentRepository
    {
        Task<CommentResponseDTO> AddComment(CommentCreateDTO comment);
        Task<CommentResponseDTO> UpdateComment(int commentId, CommentCreateDTO blogForUpdate);
        Task<(int, List<CommentResponseDTO>)> GetCommentsByBlogId(int blogId, int? pageNumber, int? pageSize, SortType? sortBy);
        Task<CommentResponseDTO> GetCommentById(int commentId);
        Task<List<CommentResponseDTO>> GetCommentsByUserId(string userId);
        Task<bool> DeleteComment(int commentId);
    }
}
