using BisleriumBlog.Application.DTOs.BlogReactionDTO;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Infrastructure.Data;
using BisleriumBlog.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace BisleriumBlog.Infrastructure.Repositories
{
    public class BlogReactionRepository : IBlogReactionRepository
    {
        private readonly AppDbContext _appDbContext;
        public BlogReactionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> ToggleUpvote(int blogId, string userId)
        {
            await CheckUserExists(userId);
            await CheckBlogExists(blogId);

            var existingReaction = await _appDbContext.BlogReactions
                .FirstOrDefaultAsync(x=>x.BlogId==blogId && x.UserId == userId);

            if (existingReaction != null)
            {
                if (existingReaction.Type == ReactionType.Upvote)
                {
                    _appDbContext.BlogReactions.Remove(existingReaction);
                    await _appDbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    existingReaction.Type = ReactionType.Upvote;
                    existingReaction.ReactedAt = DateTime.Now;
                    await _appDbContext.SaveChangesAsync();
                }
            }
            else
            {
                var newReaction = new BlogReaction
                {
                    BlogId = blogId,
                    UserId = userId,
                    Type = ReactionType.Upvote,
                    ReactedAt = DateTime.Now
                };
                await _appDbContext.BlogReactions.AddAsync(newReaction);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ToggleDownvote(int blogId, string userId)
        {
            await CheckUserExists(userId);
            await CheckBlogExists(blogId);

            var existingReaction = await _appDbContext.BlogReactions
                .FirstOrDefaultAsync(x => x.BlogId == blogId && x.UserId == userId);

            if (existingReaction != null)
            {
                if (existingReaction.Type == ReactionType.Downvote)
                {
                    _appDbContext.BlogReactions.Remove(existingReaction);
                    await _appDbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    existingReaction.Type = ReactionType.Downvote;
                    existingReaction.ReactedAt = DateTime.Now;
                    await _appDbContext.SaveChangesAsync();
                }
            }
            else
            {
                var newReaction = new BlogReaction
                {
                    BlogId = blogId,
                    UserId = userId,
                    Type = ReactionType.Downvote,
                    ReactedAt = DateTime.Now
                };
                await _appDbContext.BlogReactions.AddAsync(newReaction);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<BlogReactionDTO> GetReactionById(int id)
        {
            var reaction = await _appDbContext.BlogReactions.Where(x => x.Id == id ).SingleOrDefaultAsync();

            if (reaction != null)
                return MapperlyMapper.BlogReactionToBlogReactionDTO(reaction);

            throw new Exception($"Could not find Reaction with the id {id}");
        }

        public async Task<BlogReactionDTO> GetReactionByUserIdAndBlogId(string userId, int blogId)
        {
            await CheckUserExists(userId);
            await CheckBlogExists(blogId);

            var reaction = await _appDbContext.BlogReactions.Where(x=> x.BlogId == blogId&&x.UserId==userId).SingleOrDefaultAsync();

            if (reaction != null)
                return MapperlyMapper.BlogReactionToBlogReactionDTO(reaction);

            throw new Exception("The user has not reacted in the blog.");
        }

        public async Task<(int totalUpvotes, int totalDownvotes, List<BlogReactionDTO>)> GetReactionsByBlogId(int blogId)
        {
            await CheckBlogExists(blogId);

            var reactions = await _appDbContext.BlogReactions.Where(x => x.BlogId == blogId).ToListAsync();
            int totalUpvotes = reactions.Count(x => x.Type == ReactionType.Upvote);
            int totalDownvotes = reactions.Count(x => x.Type == ReactionType.Downvote);
            var reactionDTOs = reactions.Select(MapperlyMapper.BlogReactionToBlogReactionDTO).ToList();

            return (totalUpvotes, totalDownvotes, reactionDTOs);
        }

        private async Task CheckUserExists(string userId)
        {
            bool userExists = await _appDbContext.Users.AnyAsync(x => x.Id == userId);
            if (!userExists)
                throw new Exception("The user with the provided ID does not exist.");
        }
        private async Task CheckBlogExists(int blogId)
        {
            bool blogExists = await _appDbContext.Blogs.AnyAsync(x => x.Id == blogId && !x.IsDeleted);
            if (!blogExists)
                throw new Exception($"The blog with id {blogId} does not exist.");
        }
    }
}
