using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IBlogReactionRepository
    {
        Task<BlogReactionDTO> ToggleUpVote(int blogId, BlogReactionDTO reactionForUpdate);
        Task<BlogReactionDTO> ToggleDownVote(int blogId, BlogReactionDTO reactionForUpdate);
        Task<List<BlogReactionDTO>> GetBlogReactionByBlogId(int blogId);
        Task<BlogReactionDTO> GetBlogReactionByUserIdAndBlogId(string userId);
        Task<bool> DeleteBlogReaction(int blogReactionId);
    }
}
