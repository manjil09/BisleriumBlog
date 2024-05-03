using BisleriumBlog.Application.DTOs;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IBlogReactionRepository
    {
        Task<BlogReactionDTO> AddReaction(BlogReactionDTO blogReaction);
        Task<BlogReactionDTO> UpdateReaction(int blogId, BlogReactionDTO updatedReaction);
        Task<bool> DeleteReaction(int blogReactionId);
        Task<BlogReactionDTO> GetReactionById(int blogReactionId);
        Task<(int totalUpvotes, int totalDownvotes, List<BlogReactionDTO>)> GetReactionsByBlogId(int blogId);
        Task<BlogReactionDTO> GetReactionByUserIdAndBlogId(string userId, int blogId);
    }
}
