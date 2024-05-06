using BisleriumBlog.Application.DTOs.BlogDTO;
using BisleriumBlog.Application.DTOs.BlogReactionDTO;
using BisleriumBlog.Application.DTOs.CommentDTO;
using BisleriumBlog.Application.DTOs.CommentReactionDTO;
using BisleriumBlog.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace BisleriumBlog.Infrastructure.Mapper
{
    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public static partial class MapperlyMapper
    {
        //public static partial BlogCreateDTO BlogToBlogCreateDTO(Blog blog);
        public static partial Blog BlogCreateDTOToBlog(BlogCreateDTO blogDto);
        public static partial BlogResponseDTO BlogToBlogResponseDTO(Blog blog);
        [MapperIgnoreTarget(nameof(BlogHistory.Id))]
        [MapProperty(source: nameof(Blog.Id), target: nameof(BlogHistory.BlogId))]
        public static partial BlogHistory BlogToBlogHistory(Blog blog);

        public static partial Comment CommentCreateDTOToComment(CommentCreateDTO commentDto);
        public static partial CommentResponseDTO CommentToCommentResponseDTO(Comment comment);
        [MapperIgnoreTarget(nameof(CommentHistory.Id))]
        [MapProperty(source: nameof(Comment.Id), target: nameof(CommentHistory.CommentId))]
        public static partial CommentHistory CommentToCommentHistory(Comment comment);

        public static partial BlogReactionDTO BlogReactionToBlogReactionDTO(BlogReaction blogReaction);
        public static partial CommentReactionDTO CommentReactionToCommentReactionDTO(CommentReaction commentReaction);
    }
}
