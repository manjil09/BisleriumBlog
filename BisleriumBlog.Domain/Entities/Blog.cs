using BisleriumBlog.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Domain.Entities
{
    public class Blog: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<Comment> Comments{ get; set; } //referencing one to many relation with comment entity
        public List<BlogReaction> Reactions { get; set; } //referencing one to many relation with blogReaction entity
        public List<BlogHistory> History { get; set; } //referencing one to many relation with blogReaction entity
        public ApplicationUser User { get; set; }
    }
}
