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
            var newBlog = new Blog()
            {
                Title = blog.Title,
                Body = blog.Body,
                Image = blog.Image,
                UserId = blog.UserId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            //var newBlog = MapperlyMapper.BlogDTOToBlog(blog);

            await appDbContext.Blogs.AddAsync(newBlog);
            await appDbContext.SaveChangesAsync();

            return blog;
        }

        public async Task<bool> DeleteBlog(int blogId)
        {
            var blogToDelete = await appDbContext.Blogs.FindAsync(blogId);
            if (blogToDelete == null)
                return false;

            blogToDelete.IsDeleted = true;

            await AddToBlogHistory(blogToDelete);
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<(int, List<BlogDTO>)> GetAllBlogs(int? pageIndex, int? pageSize, SortType? sortBy = SortType.Recency)
        {
            IQueryable<Blog> blogQuery = appDbContext.Blogs.Where(x => !x.IsDeleted);

            switch (sortBy)
            {
                case SortType.Popularity:

                    throw new NotImplementedException();
                    break;
                case SortType.Random:
                    blogQuery = blogQuery.OrderBy(x => Guid.NewGuid()); // To sort randomly
                    break;
                case SortType.Recency:
                default:
                    blogQuery = blogQuery.OrderByDescending(x => x.UpdatedAt); // To sort by descending creation date
                    break;
            }

            //var blogs = await blogQuery.ToListAsync();
            var paginatedBlogs = await PaginatedList<Blog>.CreateAsync(blogQuery, pageIndex ?? 1, pageSize ?? 10);
            int totalPages = paginatedBlogs.TotalPages;

            var blogDTOs = paginatedBlogs.Select(MapperlyMapper.BlogToBlogDTO).ToList();

            return (totalPages, blogDTOs);
        }

        public async Task<BlogDTO> GetBlogById(int id)
        {
            var blog = await appDbContext.Blogs.Where(x => x.Id == id && !x.IsDeleted).SingleOrDefaultAsync();

            if (blog != null && !blog.IsDeleted)
                return MapperlyMapper.BlogToBlogDTO(blog);

            throw new KeyNotFoundException($"Could not find Blog with the id {id}");
        }

        public async Task<List<BlogDTO>> GetBlogsByUserId(string userId)
        {
            var blogs = await appDbContext.Blogs.Where(x => x.UserId == userId && !x.IsDeleted).ToListAsync();
            if (blogs != null)
            {
                var blogDTOs = blogs.Select(MapperlyMapper.BlogToBlogDTO).ToList();
                return blogDTOs;
            }
            throw new Exception("The user has not created any blogs.");
        }

        public async Task<BlogDTO> UpdateBlog(int blogId, BlogDTO updatedBlog)
        {
            var blogForUpdate = await appDbContext.Blogs.Where(x => x.Id == blogId && !x.IsDeleted).SingleOrDefaultAsync();
            if (blogForUpdate != null)
            {
                await AddToBlogHistory(blogForUpdate);

                blogForUpdate.Title = updatedBlog.Title;
                blogForUpdate.Body = updatedBlog.Body;
                blogForUpdate.Image = updatedBlog.Image;
                blogForUpdate.UpdatedAt = DateTime.Now;

                await appDbContext.SaveChangesAsync();

                return MapperlyMapper.BlogToBlogDTO(blogForUpdate);
            }

            throw new KeyNotFoundException($"Could not find Blog with the id {blogId}");
        }

        public async Task AddToBlogHistory(Blog blog)
        {
            var blogHistory = MapperlyMapper.BlogToBlogHistory(blog);
            blogHistory.CreatedAt = DateTime.Now;

            await appDbContext.BlogHistory.AddAsync(blogHistory);
        }
    }
}
