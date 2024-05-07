using Azure;
using BisleriumBlog.Application.Common;
using BisleriumBlog.Application.DTOs.CommentDTO;
using BisleriumBlog.Application.Interfaces.IRepositories;
using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Infrastructure.Data;
using BisleriumBlog.Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BisleriumBlog.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _appDbContext;
        private const int UpvoteWeightage = 2;
        private const int DownvoteWeightage = -1;
        public CommentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<CommentResponseDTO> AddComment(CommentCreateDTO comment)
        {
            bool userExists = await _appDbContext.Users.AnyAsync(x => x.Id == comment.UserId);
            if (!userExists)
                throw new Exception("The user with the provided ID does not exist.");


            var newComment = MapperlyMapper.CommentCreateDTOToComment(comment);
            newComment.CreatedAt = DateTime.Now;
            newComment.UpdatedAt = DateTime.Now;

            await _appDbContext.Comments.AddAsync(newComment);
            await _appDbContext.SaveChangesAsync();
            return MapperlyMapper.CommentToCommentResponseDTO(newComment);
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            var commentToDelete = await _appDbContext.Comments.FindAsync(commentId);
            if (commentToDelete == null)
                return false;

            commentToDelete.IsDeleted = true;

            await AddToCommentHistory(commentToDelete);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CommentResponseDTO> GetCommentById(int commentId)
        {
            var comment = await _appDbContext.Comments.Where(x => x.Id == commentId && !x.IsDeleted)
                .Include(x => x.User)
                .Include(x => x.Reactions).SingleOrDefaultAsync();

            if (comment != null && !comment.IsDeleted)
            {
                var response = MapToCommentResponseDTO(comment);
                response.UserName = comment.User?.UserName ?? throw new Exception("Comment creater not found.");
                return response;
            }
            throw new KeyNotFoundException($"Could not find Comment with the id {commentId}");
        }

        public async Task<(int, List<CommentResponseDTO>)> GetCommentsByBlogId(int blogId, int? pageIndex, int? pageSize)
        {
            bool blogExists = await _appDbContext.Blogs.AnyAsync(x => x.Id == blogId && !x.IsDeleted);
            if (!blogExists)
                throw new Exception($"The blog with id {blogId} does not exist.");

            IQueryable<Comment> commentQuery = _appDbContext.Comments
                .Where(x => x.BlogId == blogId && !x.IsDeleted)
                .Include(x => x.User)
                .Include (x => x.Reactions)
                .OrderByDescending(x =>
                    x.Reactions.Count(r => r.Type == ReactionType.Upvote) * UpvoteWeightage +
                    x.Reactions.Count(r => r.Type == ReactionType.Downvote) * DownvoteWeightage);
                

            var paginatedComments = await PaginatedList<Comment>.CreateAsync(commentQuery, pageIndex ?? 1, pageSize ?? 10);
            int totalPages = paginatedComments.TotalPages;

            var commentDTOs = paginatedComments.Select(MapToCommentResponseDTO).ToList();

            return (totalPages, commentDTOs);
        }

        public async Task<CommentResponseDTO> GetCommentByUserIdAndBlogId(string userId, int blogId)
        {
            var comment = await _appDbContext.Comments.Where(x => x.UserId == userId && x.BlogId == blogId && !x.IsDeleted)
                .Include(x => x.User)
                .Include(x => x.Reactions).SingleOrDefaultAsync();

            if (comment != null)
            {
                var response = MapToCommentResponseDTO(comment);
                response.UserName = comment.User?.UserName ?? throw new Exception("Comment creater not found.");
                return response;
            }

            throw new KeyNotFoundException($"Current user has not posted any comment on the blog.");
        }

        public async Task<CommentResponseDTO> UpdateComment(int commentId, CommentUpdateDTO updatedComment)
        {
            var commentForUpdate = await _appDbContext.Comments.Where(x => x.Id == commentId && !x.IsDeleted)
                .Include(x => x.User)
                .Include(x => x.Reactions).SingleOrDefaultAsync();
            if (commentForUpdate != null)
            {
                await AddToCommentHistory(commentForUpdate);

                commentForUpdate.Body = updatedComment.Body;
                commentForUpdate.UpdatedAt = DateTime.Now;

                await _appDbContext.SaveChangesAsync();

                return MapToCommentResponseDTO(commentForUpdate);
            }

            throw new KeyNotFoundException($"Could not find Comment with the id {commentId}");
        }

        private async Task AddToCommentHistory(Comment comment)
        {
            var commentHistory = MapperlyMapper.CommentToCommentHistory(comment);
            commentHistory.CreatedAt = DateTime.Now;

            await _appDbContext.CommentHistory.AddAsync(commentHistory);
        }

        private CommentResponseDTO MapToCommentResponseDTO(Comment comment)
        {
            var commentDTO = MapperlyMapper.CommentToCommentResponseDTO(comment);
            commentDTO.UserName = comment.User?.UserName ?? throw new Exception("Comment creater not found.");
            commentDTO.TotalDownvotes = comment.Reactions.Where(x => x.Type == ReactionType.Downvote).Count();
            commentDTO.TotalUpvotes = comment.Reactions.Where(x => x.Type == ReactionType.Upvote).Count();
            return commentDTO;
        }
    }
}
