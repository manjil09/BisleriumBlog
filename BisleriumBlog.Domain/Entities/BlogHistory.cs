using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Domain.Entities
{
    public class BlogHistory
    {
        [Key]
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Blog Blog { get; set; } //Reference to dependent entity
    }
}
