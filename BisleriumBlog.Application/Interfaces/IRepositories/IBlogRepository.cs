using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IBlogRepository
    {
        Task<BlogDTO> AddBlog(BlogDTO blog);
        Task<BlogDTO> UpdateBlog(int blogId, BlogDTO updatedBlog);
        Task<(int, List<BlogDTO>)> GetAllBlogs(int? pageNumber, int? pageSize, SortType? sortBy);
        Task<BlogDTO> GetBlogById(int blogId);
        Task<List<BlogDTO>> GetBlogsByUserId(string userId);
        Task<bool> DeleteBlog(int blogId);
    }
}
