using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
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

        public async Task<BlogReactionDTO> AddReaction(BlogReactionDTO blogReaction)
        {
            var newReaction = MapperlyMapper.BlogReactionDTOToBlogReaction(blogReaction);
            newReaction.ReactedAt = DateTime.Now;

            await _appDbContext.BlogReactions.AddAsync(newReaction);
            await _appDbContext.SaveChangesAsync();

            return blogReaction;
        }

        public async Task<bool> DeleteReaction(int blogReactionId)
        {
            var reactionToDelete = await _appDbContext.BlogReactions.FindAsync(blogReactionId);
            if (reactionToDelete != null)
            {
                _appDbContext.BlogReactions.Remove(reactionToDelete);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public Task<BlogReactionDTO> GetReactionById(int blogReactionId)
        {
            throw new NotImplementedException();
        }

        public Task<BlogReactionDTO> GetReactionByUserIdAndBlogId(string userId, int blogId)
        {
            throw new NotImplementedException();
        }

        public async Task<(int totalUpvotes, int totalDownvotes, List<BlogReactionDTO>)> GetReactionsByBlogId(int blogId)
        {
            bool blogExists = await _appDbContext.Blogs.AnyAsync(x=>x.Id == blogId);
            if (!blogExists)
                throw new Exception($"The blog with id {blogId} does not exist.");

            var reactions = await _appDbContext.BlogReactions.Where(x => x.BlogId == blogId).ToListAsync();
            int totalUpvotes = reactions.Count(x => x.Type == ReactionType.Upvote);
            int totalDownvotes = reactions.Count(x => x.Type == ReactionType.Downvote);
            var reactionDTOs = reactions.Select(MapperlyMapper.BlogReactionToBlogReactionDTO).ToList();

            return (totalUpvotes, totalDownvotes, reactionDTOs);
        }

        public Task<BlogReactionDTO> UpdateReaction(int blogId, BlogReactionDTO updatedReaction)
        {
            throw new NotImplementedException();
        }
    }
}
