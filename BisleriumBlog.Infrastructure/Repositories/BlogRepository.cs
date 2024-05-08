using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.BlogDTO;
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

        public async Task<(int, List<BlogResponseDTO>)> GetAllBlogs(int? pageIndex, int? pageSize, SortType? sortBy = SortType.Recency, bool isAscending = false)
        {
            IQueryable<Blog> blogQuery = _appDbContext.Blogs.Where(x => !x.IsDeleted).Include(x => x.Reactions)
                .Include(x => x.Comments);

            switch (sortBy)
            {
                case SortType.Popularity:
                    blogQuery = SortByPopularity(blogQuery, isAscending);
                    break;
                case SortType.Random:
                    blogQuery = blogQuery.OrderBy(x => Guid.NewGuid()); // To sort randomly
                    break;
                case SortType.Recency:
                default:
                    if (isAscending)
                        blogQuery = blogQuery.OrderBy(x => x.UpdatedAt); // To sort by ascending creation date
                    else
                        blogQuery = blogQuery.OrderByDescending(x => x.UpdatedAt); // To sort by descending creation date
                    break;
            }
            blogQuery = blogQuery.Include(x => x.User);
            var paginatedBlogs = await PaginatedList<Blog>.CreateAsync(blogQuery, pageIndex ?? 1, pageSize ?? 10);
            int totalPages = paginatedBlogs.TotalPages;

            var blogDTOs = paginatedBlogs.Select(MapToBlogResponseDTO).ToList();

            return (totalPages, blogDTOs);
        }


        public async Task<(int, List<BlogResponseDTO>)> GetAllBlogsByMonth(int month, int year, int? pageIndex, int? pageSize, SortType? sortBy = SortType.Recency, bool isAscending = false)
        {
            IQueryable<Blog> blogQuery = _appDbContext.Blogs.Where(x => !x.IsDeleted && x.CreatedAt.Month == month && x.CreatedAt.Year == year)
                .Include(x => x.Reactions)
                .Include(x => x.Comments);

            switch (sortBy)
            {
                case SortType.Popularity:
                    blogQuery = SortByPopularity(blogQuery, isAscending);
                    break;
                case SortType.Random:
                    blogQuery = blogQuery.OrderBy(x => Guid.NewGuid()); // To sort randomly
                    break;
                case SortType.Recency:
                default:
                    if (isAscending)
                        blogQuery = blogQuery.OrderBy(x => x.UpdatedAt); // To sort by ascending creation date
                    else
                        blogQuery = blogQuery.OrderByDescending(x => x.UpdatedAt); // To sort by descending creation date
                    break;
            }
            blogQuery = blogQuery.Include(x => x.User);
            var paginatedBlogs = await PaginatedList<Blog>.CreateAsync(blogQuery, pageIndex ?? 1, pageSize ?? 10);
            int totalPages = paginatedBlogs.TotalPages;

            var blogDTOs = paginatedBlogs.Select(MapToBlogResponseDTO).ToList();

            return (totalPages, blogDTOs);
        }

        public async Task<BlogResponseDTO> GetBlogById(int id)
        {
            var blog = await _appDbContext.Blogs.Where(x => x.Id == id && !x.IsDeleted)
                .Include(x => x.Reactions)
                .Include(x => x.Comments)
                .Include(x => x.User)
                .SingleOrDefaultAsync();

            if (blog != null)
                return MapToBlogResponseDTO(blog);

            throw new Exception($"Could not find Blog with the id {id}");
        }

        public async Task<List<BlogResponseDTO>> GetBlogsByUserId(string userId)
        {
            bool userExists = await _appDbContext.Users.AnyAsync(x => x.Id == userId);
            if (!userExists)
                throw new Exception("The user with the provided ID does not exist.");

            var blogs = await _appDbContext.Blogs.Where(x => x.UserId == userId && !x.IsDeleted)
                .Include(x => x.Reactions)
                .Include(x => x.Comments)
                .Include(x => x.User).ToListAsync();
            if (!blogs.IsNullOrEmpty())
            {
                var blogDTOs = blogs.Select(MapToBlogResponseDTO).ToList();
                return blogDTOs;
            }
            throw new Exception("The user has not created any blogs.");
        }

        public async Task<BlogResponseDTO> UpdateBlog(int blogId, BlogUpdateDTO updatedBlog, string imageUrl)
        {
            var blogForUpdate = await _appDbContext.Blogs.Where(x => x.Id == blogId && !x.IsDeleted)
                .Include(x => x.Reactions)
                .Include(x => x.Comments)
                .Include(x => x.User).SingleOrDefaultAsync();
            if (blogForUpdate != null)
            {
                await AddToBlogHistory(blogForUpdate);

                blogForUpdate.Title = updatedBlog.Title;
                blogForUpdate.Body = updatedBlog.Body;
                blogForUpdate.ImageUrl = imageUrl;
                blogForUpdate.UpdatedAt = DateTime.Now;

                await _appDbContext.SaveChangesAsync();

                return MapToBlogResponseDTO(blogForUpdate);
            }

            throw new KeyNotFoundException($"Could not find Blog with the id {blogId}");
        }

        public async Task AddToBlogHistory(Blog blog)
        {
            var blogHistory = MapperlyMapper.BlogToBlogHistory(blog);
            blogHistory.CreatedAt = DateTime.Now;

            await _appDbContext.BlogHistory.AddAsync(blogHistory);
        }
        private IQueryable<Blog> SortByPopularity(IQueryable<Blog> blogQuery, bool isAscending)
        {
            if (isAscending)
            {
                blogQuery = blogQuery
                    .OrderBy(x =>
                    x.Reactions.Count(r => r.Type == ReactionType.Upvote) * UpvoteWeightage +
                    x.Reactions.Count(r => r.Type == ReactionType.Downvote) * DownvoteWeightage +
                    x.Comments.Count(c => !c.IsDeleted) * CommentWeightage);

                return blogQuery;
            }
            else
            {
                blogQuery = blogQuery
                    .OrderByDescending(x =>
                    x.Reactions.Count(r => r.Type == ReactionType.Upvote) * UpvoteWeightage +
                    x.Reactions.Count(r => r.Type == ReactionType.Downvote) * DownvoteWeightage +
                    x.Comments.Count(c => !c.IsDeleted) * CommentWeightage);

                return blogQuery;
            }
        }
        private BlogResponseDTO MapToBlogResponseDTO(Blog blog)
        {
            var blogDTO = MapperlyMapper.BlogToBlogResponseDTO(blog);
            blogDTO.UserName = blog.User?.UserName ?? throw new Exception("Blog creater not found.");
            blogDTO.TotalDownvotes = blog.Reactions.Where(x => x.Type == ReactionType.Downvote).Count();
            blogDTO.TotalUpvotes = blog.Reactions.Where(x => x.Type == ReactionType.Upvote).Count();
            blogDTO.TotalComments = blog.Comments.Count();
            blogDTO.Popularity = blogDTO.TotalUpvotes * UpvoteWeightage + blogDTO.TotalDownvotes * DownvoteWeightage + blogDTO.TotalComments * CommentWeightage;
            return blogDTO;
        }
    }
}
