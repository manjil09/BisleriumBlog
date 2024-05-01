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
        private readonly AppDbContext _appDbContext;

        public BlogRepository(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public async Task<BlogResponseDTO> AddBlog(BlogCreateDTO blog)
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

            await _appDbContext.Blogs.AddAsync(newBlog);
            await _appDbContext.SaveChangesAsync();

            return MapperlyMapper.BlogToBlogResponseDTO(newBlog);
        }

        public async Task<bool> DeleteBlog(int blogId)
        {
            var blogToDelete = await _appDbContext.Blogs.FindAsync(blogId);
            if (blogToDelete == null)
                return false;

            blogToDelete.IsDeleted = true;

            await AddToBlogHistory(blogToDelete);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<(int, List<BlogResponseDTO>)> GetAllBlogs(int? pageIndex, int? pageSize, SortType? sortBy = SortType.Recency)
        {
            IQueryable<Blog> blogQuery = _appDbContext.Blogs.Where(x => !x.IsDeleted);

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

            var blogDTOs = paginatedBlogs.Select(MapperlyMapper.BlogToBlogResponseDTO).ToList();

            return (totalPages, blogDTOs);
        }

        public async Task<BlogResponseDTO> GetBlogById(int id)
        {
            var blog = await _appDbContext.Blogs.Where(x => x.Id == id && !x.IsDeleted).SingleOrDefaultAsync();

            if (blog != null && !blog.IsDeleted)
                return MapperlyMapper.BlogToBlogResponseDTO(blog);

            throw new KeyNotFoundException($"Could not find Blog with the id {id}");
        }

        public async Task<List<BlogResponseDTO>> GetBlogsByUserId(string userId)
        {
            var blogs = await _appDbContext.Blogs.Where(x => x.UserId == userId && !x.IsDeleted).ToListAsync();
            if (blogs != null)
            {
                var blogDTOs = blogs.Select(MapperlyMapper.BlogToBlogResponseDTO).ToList();
                return blogDTOs;
            }
            throw new Exception("The user has not created any blogs.");
        }

        public async Task<BlogResponseDTO> UpdateBlog(int blogId, BlogCreateDTO updatedBlog)
        {
            var blogForUpdate = await _appDbContext.Blogs.Where(x => x.Id == blogId && !x.IsDeleted).SingleOrDefaultAsync();
            if (blogForUpdate != null)
            {
                await AddToBlogHistory(blogForUpdate);

                blogForUpdate.Title = updatedBlog.Title;
                blogForUpdate.Body = updatedBlog.Body;
                blogForUpdate.Image = updatedBlog.Image;
                blogForUpdate.UpdatedAt = DateTime.Now;

                await _appDbContext.SaveChangesAsync();

                return MapperlyMapper.BlogToBlogResponseDTO(blogForUpdate);
            }

            throw new KeyNotFoundException($"Could not find Blog with the id {blogId}");
        }

        public async Task AddToBlogHistory(Blog blog)
        {
            var blogHistory = MapperlyMapper.BlogToBlogHistory(blog);
            blogHistory.CreatedAt = DateTime.Now;

            await _appDbContext.BlogHistory.AddAsync(blogHistory);
        }
    }
}
