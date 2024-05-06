using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.BlogReactionDTO;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IBlogReactionRepository
    {
        Task<Response<string>> ToggleUpvote(int blogId, string userId);
        Task<Response<string>> ToggleDownvote(int blogId, string userId);
        Task<BlogReactionDTO> GetReactionById(int blogReactionId);
        Task<(int totalUpvotes, int totalDownvotes, List<BlogReactionDTO>)> GetReactionsByBlogId(int blogId);
        Task<BlogReactionDTO> GetReactionByUserIdAndBlogId(string userId, int blogId);
    }
}
