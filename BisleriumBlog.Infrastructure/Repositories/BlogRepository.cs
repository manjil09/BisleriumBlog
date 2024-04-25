using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Infrastructure.Data;

namespace BisleriumBlog.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext appDbContext;

        public BlogRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<BlogDTO> AddBlog(BlogDTO blog)
        {
            var newBlog = new Blog()
            {
                Title = blog.Title,
                Body = blog.Body,
                Image = blog.Image,
                UserId = blog.UserId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            await appDbContext.Blogs.AddAsync(newBlog);
            await appDbContext.SaveChangesAsync();

            var result = new BlogDTO()
            {
                Title = newBlog.Title,
                Body = newBlog.Body,
                Image = newBlog.Image,
                UserId = blog.UserId,
            };

            return result;
        }

        public Task<List<BlogDTO>> GetAllBlogs()
        {
            throw new NotImplementedException();
        }

        public Task<BlogDTO> GetBlogById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
