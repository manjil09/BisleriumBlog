using BisleriumBlog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Application.DTOs.BlogReactionDTO
{
    public class BlogReactionResponseDTO
    {
        public int TotalUpvotes { get; set; }
        public int TotalDownvotes { get; set; }
        public string BlogCreatorId { get; set; }
        public ReactionType Type { get; set; }
    }
}
