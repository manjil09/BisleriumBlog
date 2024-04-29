using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Infrastructure.Data;
using BisleriumBlog.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

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
            //var newBlog = new Blog()
            //{
            //    Title = blog.Title,
            //    Body = blog.Body,
            //    Image = blog.Image,
            //    UserId = blog.UserId,
            //    CreatedAt = DateTime.Now,
            //    UpdatedAt = DateTime.Now,
            //};
            var newBlog = MapperlyMapper.BlogDTOToBlog(blog);

            await appDbContext.Blogs.AddAsync(newBlog);
            await appDbContext.SaveChangesAsync();

            return blog;
        }

        public async Task<bool> DeleteBlog(int blogId)
        {
            var blogToDelete = await appDbContext.Blogs.FindAsync(blogId);
            if (blogToDelete == null)
                return false;

            appDbContext.Blogs.Remove(blogToDelete);
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<BlogDTO>> GetAllBlogs(int? pageIndex, int? pageSize, SortType? sortBy = SortType.Random)
        {
            IQueryable<Blog> blogQuery = appDbContext.Blogs;
            
            switch (sortBy)
            {
                case SortType.Recency:
                    blogQuery = blogQuery.OrderByDescending(x => x.CreatedAt); // To sort by descending creation date
                    break;
                case SortType.Popularity:

                    throw new NotImplementedException();
                    break;
                case SortType.Random:
                default:
                    blogQuery = blogQuery.OrderBy(x => Guid.NewGuid()); // To sort randomly
                    break;
            }

            //var blogs = await blogQuery.ToListAsync();
            var paginatedBlogs = await PaginatedList<Blog>.CreateAsync(blogQuery, pageIndex ?? 1, pageSize ?? 10);

            var blogDTOs = paginatedBlogs.Select(MapperlyMapper.BlogToBlogDTO).ToList();

            return blogDTOs;
        }

        public async Task<BlogDTO> GetBlogById(int id)
        {
            var blog = await appDbContext.Blogs.FindAsync(id);

            if (blog != null)
                return MapperlyMapper.BlogToBlogDTO(blog);

            throw new KeyNotFoundException($"Could not find Blog with the id {id}");
        }

        public Task<List<BlogDTO>> GetBlogsByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<BlogDTO> UpdateBlog(int blogId, BlogDTO updatedBlog)
        {
            var blogForUpdate = await appDbContext.Blogs.FindAsync(blogId);
            if (blogForUpdate != null) { }

            throw new NotImplementedException();

        }
    }
}
