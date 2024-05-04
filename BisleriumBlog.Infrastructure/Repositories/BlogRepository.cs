using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Infrastructure.Data;
using BisleriumBlog.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BisleriumBlog.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _appDbContext;

        private const int UpvoteWeightage = 2;
        private const int DownvoteWeightage = -1;
        private const int CommentWeightage = 1;

        public BlogRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<BlogResponseDTO> AddBlog(BlogCreateDTO blog, string imageUrl)
        {
            bool userExists = await _appDbContext.Users.AnyAsync(x => x.Id == blog.UserId);
            if (!userExists)
                throw new Exception("The user with the provided ID does not exist.");

            var newBlog = MapperlyMapper.BlogCreateDTOToBlog(blog);
            newBlog.ImageUrl = imageUrl;
            newBlog.CreatedAt = DateTime.Now;
            newBlog.UpdatedAt = DateTime.Now;

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
                    blogQuery = SortByPopularity(blogQuery);
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

            if (blog != null)
                return MapperlyMapper.BlogToBlogResponseDTO(blog);

            throw new KeyNotFoundException($"Could not find Blog with the id {id}");
        }

        public async Task<List<BlogResponseDTO>> GetBlogsByUserId(string userId)
        {
            bool userExists = await _appDbContext.Users.AnyAsync(x => x.Id == userId);
            if (!userExists)
                throw new Exception("The user with the provided ID does not exist.");

            var blogs = await _appDbContext.Blogs.Where(x => x.UserId == userId && !x.IsDeleted).ToListAsync();
            if (!blogs.IsNullOrEmpty())
            {
                var blogDTOs = blogs.Select(MapperlyMapper.BlogToBlogResponseDTO).ToList();
                return blogDTOs;
            }
            throw new Exception("The user has not created any blogs.");
        }

        public async Task<BlogResponseDTO> UpdateBlog(int blogId, BlogCreateDTO updatedBlog, string imageUrl)
        {
            var blogForUpdate = await _appDbContext.Blogs.Where(x => x.Id == blogId && !x.IsDeleted).SingleOrDefaultAsync();
            if (blogForUpdate != null)
            {
                await AddToBlogHistory(blogForUpdate);

                blogForUpdate.Title = updatedBlog.Title;
                blogForUpdate.Body = updatedBlog.Body;
                blogForUpdate.ImageUrl = imageUrl;
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
        public IQueryable<Blog> SortByPopularity(IQueryable<Blog> blogQuery)
        {
            blogQuery = blogQuery.Include(x => x.Reactions)
                .Include(x => x.Comments)
                .OrderByDescending(x =>
                x.Reactions.Count(r => r.Type == ReactionType.Upvote) * UpvoteWeightage +
                x.Reactions.Count(r => r.Type == ReactionType.Downvote) * DownvoteWeightage +
                x.Comments.Count(c => !c.IsDeleted) * CommentWeightage);

            return blogQuery;
        }
    }
}
