using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace BisleriumBlog.Infrastructure.Mapper
{
    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public static partial class MapperlyMapper
    {
        public static partial BlogDTO BlogToBlogDTO(Blog blog);
        public static partial Blog BlogDTOToBlog(BlogDTO blogDto);

        [MapperIgnoreTarget(nameof(BlogHistory.Id))]
        [MapProperty(source: nameof(Blog.Id), target: nameof(BlogHistory.BlogId))]
        public static partial BlogHistory BlogToBlogHistory(Blog blog);

        public static partial CommentDTO CommentToCommentDTO(Comment comment);
        public static partial Comment CommentDTOToComment(CommentDTO commentDto);

        [MapperIgnoreTarget(nameof(CommentHistory.Id))]
        [MapProperty(source: nameof(Comment.Id), target: nameof(CommentHistory.CommentId))]
        public static partial CommentHistory CommentToCommentHistory(Comment comment);
    }
}
