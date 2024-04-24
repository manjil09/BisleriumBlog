using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext appDbContext;

        public BlogRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Blog> AddBlog(Blog blog)
        {
            var addBlog = new Blog()
            {
                Title = blog.Title,
                Body = blog.Body,
                Image = blog.Image,
                UserId = blog.UserId,
            };

            await appDbContext.Blogs.AddAsync(addBlog);
            await appDbContext.SaveChangesAsync();

            return addBlog;
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
