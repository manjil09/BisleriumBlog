using BisleriumBlog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumBlog.Domain.Shared
{
    public abstract class ReactionBase
    {
        [Key]
        public int Id { get; set; }
        public ReactionType Type { get; set; }
        public DateTime ReactedAt { get; set; }
        public int UserId { get; set; }

        //pending reference to user table
    }
}
