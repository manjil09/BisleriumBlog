using BisleriumBlog.Application.DTOs;
using BisleriumBlog.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace BisleriumBlog.Infrastructure.Mapper
{
    [Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
    public static partial class MapperlyMapper
    {
        //public static partial BlogCreateDTO BlogToBlogCreateDTO(Blog blog);
        public static partial BlogResponseDTO BlogToBlogResponseDTO(Blog blog);
        //public static partial Blog BlogCreateDTOToBlog(BlogCreateDTO blogDto);

        [MapperIgnoreTarget(nameof(BlogHistory.Id))]
        [MapProperty(source: nameof(Blog.Id), target: nameof(BlogHistory.BlogId))]
        public static partial BlogHistory BlogToBlogHistory(Blog blog);

        public static partial CommentResponseDTO CommentToCommentResponseDTO(Comment comment);
        //public static partial Comment CommentDTOToComment(CommentCreateDTO commentDto);

        [MapperIgnoreTarget(nameof(CommentHistory.Id))]
        [MapProperty(source: nameof(Comment.Id), target: nameof(CommentHistory.CommentId))]
        public static partial CommentHistory CommentToCommentHistory(Comment comment);
    }
}
