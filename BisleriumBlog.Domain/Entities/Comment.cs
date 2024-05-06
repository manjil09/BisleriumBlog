using BisleriumBlog.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Domain.Entities
{
    public class Comment: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Body { get; set; }
        public int BlogId { get; set;}
        public string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Blog Blog { get; set; } //Reference to dependent entity
        public List<CommentReaction> Reactions { get; set; } //referencing one to many relation with commentReaction entity
        public List<CommentHistory> History { get; set; } //referencing one to many relation with commentHistory entity
        public ApplicationUser User { get; set; }
    }
}
