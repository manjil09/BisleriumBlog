using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;

namespace BisleriumBlog.Application.Interfaces.IRepositories
{
    public interface IBlogRepository
    {
        Task<BlogDTO> AddBlog(BlogDTO blog);
        Task<BlogDTO> UpdateBlog(int blogId, BlogDTO blogForUpdate);
        Task<List<BlogDTO>> GetAllBlogs(bool sortByPopularity = false);
        Task<BlogDTO> GetBlogById(int blogId);
        Task<List<BlogDTO>> GetBlogsByUserId(string userId);
        Task<bool> DeleteBlog(int blogId);
    }
}
