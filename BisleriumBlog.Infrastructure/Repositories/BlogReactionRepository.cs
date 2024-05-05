using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Infrastructure.Data;
using BisleriumBlog.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

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
            bool userExists = await _appDbContext.Users.AnyAsync(x => x.Id == blogReaction.UserId);
            if (!userExists)
                throw new Exception("The user with the provided ID does not exist.");

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
        public async Task<BlogReactionDTO> GetReactionById(int id)
        {
            var reaction = await _appDbContext.BlogReactions.Where(x => x.Id == id ).SingleOrDefaultAsync();

            if (reaction != null)
                return MapperlyMapper.BlogReactionToBlogReactionDTO(reaction);

            throw new Exception($"Could not find Reaction with the id {id}");
        }

        public async Task<BlogReactionDTO> GetReactionByUserIdAndBlogId(string userId, int blogId)
        {
            bool userExists = await _appDbContext.Users.AnyAsync(x => x.Id == userId);
            if (!userExists)
                throw new Exception("The user with the provided ID does not exist.");

            var reaction = await _appDbContext.BlogReactions.Where(x=> x.BlogId == blogId&&x.UserId==userId).SingleOrDefaultAsync();

            if (reaction != null)
                return MapperlyMapper.BlogReactionToBlogReactionDTO(reaction);

            throw new Exception("The user has not reacted in the blog.");
        }

        public async Task<(int totalUpvotes, int totalDownvotes, List<BlogReactionDTO>)> GetReactionsByBlogId(int blogId)
        {
            bool blogExists = await _appDbContext.Blogs.AnyAsync(x=>x.Id == blogId && !x.IsDeleted);
            if (!blogExists)
                throw new Exception($"The blog with id {blogId} does not exist.");

            var reactions = await _appDbContext.BlogReactions.Where(x => x.BlogId == blogId).ToListAsync();
            int totalUpvotes = reactions.Count(x => x.Type == ReactionType.Upvote);
            int totalDownvotes = reactions.Count(x => x.Type == ReactionType.Downvote);
            var reactionDTOs = reactions.Select(MapperlyMapper.BlogReactionToBlogReactionDTO).ToList();

            return (totalUpvotes, totalDownvotes, reactionDTOs);
        }

        public async Task<BlogReactionDTO> UpdateReaction(int id, BlogReactionDTO updatedReaction)
        {
            var reactionForUpdate = await _appDbContext.BlogReactions.Where(x => x.Id == id).SingleOrDefaultAsync();
            if (reactionForUpdate != null)
            {
                reactionForUpdate.Type = updatedReaction.Type;
                reactionForUpdate.ReactedAt = DateTime.Now;

                await _appDbContext.SaveChangesAsync();

                return MapperlyMapper.BlogReactionToBlogReactionDTO(reactionForUpdate);
            }

            throw new KeyNotFoundException($"Could not find Reaction with the id {id}");
        }
    }
}
