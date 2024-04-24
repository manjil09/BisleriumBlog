using BisleriumBlog.Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Domain.Entities
{
    public class CommentReaction: BaseReaction
    {
        [Key]
        public int Id { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; } //Reference to dependent entity
        public int UserId { get; set; }

        //pending reference to user table
    }
}
