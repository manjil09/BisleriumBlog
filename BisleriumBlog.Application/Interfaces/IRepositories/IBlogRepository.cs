using BisleriumBlog.Application.DTOs.BlogDTO;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IBlogRepository
    {
        Task<BlogResponseDTO> AddBlog(BlogCreateDTO blog, string imageUrl);
        Task<BlogResponseDTO> UpdateBlog(int blogId, BlogUpdateDTO updatedBlog, string imageUrl);
        Task<(int, List<BlogResponseDTO>)> GetAllBlogs(int? pageIndex, int? pageSize, SortType? sortBy, bool isAscending);
        Task<(int, List<BlogResponseDTO>)> GetAllBlogsByMonth(int month, int year, int? pageIndex, int? pageSize, SortType? sortBy = SortType.Recency, bool isAscending = false);
        Task<BlogResponseDTO> GetBlogById(int blogId);
        Task<List<BlogResponseDTO>> GetBlogsByUserId(string userId);
        Task<bool> DeleteBlog(int blogId);
    }
}
