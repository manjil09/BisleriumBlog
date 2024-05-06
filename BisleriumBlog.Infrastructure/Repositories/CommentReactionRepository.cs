using BisleriumBlog.Application.DTOs.CommentReactionDTO;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Infrastructure.Data;
using BisleriumBlog.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace BisleriumBlog.Infrastructure.Repositories
{
    public class CommentReactionRepository:ICommentReactionRepository
    {
        private readonly AppDbContext _appDbContext;

        public CommentReactionRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<CommentReactionDTO> GetReactionById(int commentReactionId)
        {
            var reaction = await _appDbContext.CommentReactions.Where(x => x.Id == commentReactionId).SingleOrDefaultAsync();

            if (reaction != null)
                return MapperlyMapper.CommentReactionToCommentReactionDTO(reaction);

            throw new Exception($"Could not find Reaction with the id {commentReactionId}");
        }

        public async Task<CommentReactionDTO> GetReactionByUserIdAndCommentId(string userId, int commentId)
        {
            bool userExists = await _appDbContext.Users.AnyAsync(x => x.Id == userId);
            if (!userExists)
                throw new Exception("The user with the provided ID does not exist.");

            var reaction = await _appDbContext.CommentReactions.Where(x => x.CommentId == commentId && x.UserId == userId).SingleOrDefaultAsync();

            if (reaction != null)
                return MapperlyMapper.CommentReactionToCommentReactionDTO(reaction);

            throw new Exception("The user has not reacted in the comment.");
        }

        public async Task<(int totalUpvotes, int totalDownvotes, List<CommentReactionDTO>)> GetReactionsByCommentId(int commentId)
        {
            bool blogExists = await _appDbContext.Comments.AnyAsync(x => x.Id == commentId && !x.IsDeleted);
            if (!blogExists)
                throw new Exception($"The comment with id {commentId} does not exist.");

            var reactions = await _appDbContext.CommentReactions.Where(x => x.CommentId == commentId).ToListAsync();
            int totalUpvotes = reactions.Count(x => x.Type == ReactionType.Upvote);
            int totalDownvotes = reactions.Count(x => x.Type == ReactionType.Downvote);
            var reactionDTOs = reactions.Select(MapperlyMapper.CommentReactionToCommentReactionDTO).ToList();

            return (totalUpvotes, totalDownvotes, reactionDTOs);
        }

        public async Task<bool> ToggleDownvote(int commentId, string userId)
        {
            bool userExists = await _appDbContext.Users.AnyAsync(x => x.Id == userId);
            if (!userExists)
                throw new Exception("The user with the provided ID does not exist.");

            var existingReaction = await _appDbContext.CommentReactions
                .FirstOrDefaultAsync(x => x.CommentId == commentId && x.UserId == userId);

            if (existingReaction != null)
            {
                if (existingReaction.Type == ReactionType.Downvote)
                {
                    _appDbContext.CommentReactions.Remove(existingReaction);
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
                var newReaction = new CommentReaction
                {
                    CommentId = commentId,
                    UserId = userId,
                    Type = ReactionType.Downvote,
                    ReactedAt = DateTime.Now
                };
                await _appDbContext.CommentReactions.AddAsync(newReaction);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ToggleUpvote(int commentId, string userId)
        {
            bool userExists = await _appDbContext.Users.AnyAsync(x => x.Id == userId);
            if (!userExists)
                throw new Exception("The user with the provided ID does not exist.");

            var existingReaction = await _appDbContext.CommentReactions
                .FirstOrDefaultAsync(x => x.CommentId == commentId && x.UserId == userId);

            if (existingReaction != null)
            {
                if (existingReaction.Type == ReactionType.Upvote)
                {
                    _appDbContext.CommentReactions.Remove(existingReaction);
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
                var newReaction = new CommentReaction
                {
                    CommentId = commentId,
                    UserId = userId,
                    Type = ReactionType.Upvote,
                    ReactedAt = DateTime.Now
                };
                await _appDbContext.CommentReactions.AddAsync(newReaction);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
