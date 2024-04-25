using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Application.DTOs
{
    public class CommentDTO
    {
        public string Body { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
    }
}
