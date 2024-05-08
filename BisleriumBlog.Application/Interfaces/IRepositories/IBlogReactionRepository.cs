using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.BlogReactionDTO;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IBlogReactionRepository
    {
        Task<BlogReactionResponseDTO> ToggleUpvote(int blogId, string userId);
        Task<BlogReactionResponseDTO> ToggleDownvote(int blogId, string userId);
        Task<BlogReactionDTO> GetReactionById(int blogReactionId);
        Task<(int totalUpvotes, int totalDownvotes, List<BlogReactionDTO>)> GetReactionsByBlogId(int blogId);
        Task<BlogReactionDTO> GetReactionByUserIdAndBlogId(string userId, int blogId);
    }
}
