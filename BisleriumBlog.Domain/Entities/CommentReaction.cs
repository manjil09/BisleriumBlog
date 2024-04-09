using BisleriumBlog.Domain.Enums;
using BisleriumBlog.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Domain.Entities
{
    public class CommentReaction: ReactionBase
    {
        public int CommentId { get; set; }

        public Comment Comment { get; set; } //Reference to dependent entity
    }
}
