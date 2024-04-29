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
        public static partial CommentDTO CommentToCommentDTO(Comment comment);
        public static partial Comment CommentDTOToComment(CommentDTO commentDto);
    }
}
