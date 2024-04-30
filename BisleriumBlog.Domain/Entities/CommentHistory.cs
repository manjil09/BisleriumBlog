using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Domain.Entities
{
    public class CommentHistory
    {
        [Key]
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } 
        public Comment Comment { get; set; } //Reference to dependent entity
    }
}
