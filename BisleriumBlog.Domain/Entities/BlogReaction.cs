using BisleriumBlog.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Domain.Entities
{
    public class BlogReaction: BaseReaction
    {
        [Key]
        public int Id { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; } //Reference to dependent entity

    }
}
