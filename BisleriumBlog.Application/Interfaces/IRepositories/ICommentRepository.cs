using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface ICommentRepository
    {
        Task<CommentDTO> AddComment(CommentDTO comment);
        Task<CommentDTO> UpdateComment(int commentId, CommentDTO blogForUpdate);
        Task<(int, List<CommentDTO>)> GetCommentsByBlogId(int blogId, int? pageNumber, int? pageSize, SortType? sortBy);
        Task<CommentDTO> GetCommentById(int commentId);
        Task<List<CommentDTO>> GetCommentsByUserId(string userId);
        Task<bool> DeleteComment(int commentId);
    }
}
