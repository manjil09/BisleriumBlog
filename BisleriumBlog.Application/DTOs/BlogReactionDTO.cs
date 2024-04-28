using BisleriumBlog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Application.DTOs
{
    public class BlogReactionDTO
    {
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public ReactionType Type { get; set; }
        public DateTime ReactedAt { get; set; }
    }
}
