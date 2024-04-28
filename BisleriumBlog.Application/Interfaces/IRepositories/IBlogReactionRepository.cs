using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IBlogReactionRepository
    {
        Task<BlogReactionDTO> AddBlogReaction(BlogReactionDTO blog);
        Task<BlogReactionDTO> ToggleBlogReaction(int blogId, BlogReactionDTO reactionForUpdate);
        Task<List<BlogReactionDTO>> GetBlogReactionByBlogId(int blogId);
        Task<List<BlogReactionDTO>> GetBlogReactionByUserId(string userId);
        Task<bool> DeleteBlogReaction(int blogReactionId);
    }
}
