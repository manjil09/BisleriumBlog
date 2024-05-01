using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IBlogRepository
    {
        Task<BlogResponseDTO> AddBlog(BlogCreateDTO blog);
        Task<BlogResponseDTO> UpdateBlog(int blogId, BlogCreateDTO updatedBlog);
        Task<(int, List<BlogResponseDTO>)> GetAllBlogs(int? pageIndex, int? pageSize, SortType? sortBy);
        Task<BlogResponseDTO> GetBlogById(int blogId);
        Task<List<BlogResponseDTO>> GetBlogsByUserId(string userId);
        Task<bool> DeleteBlog(int blogId);
    }
}
