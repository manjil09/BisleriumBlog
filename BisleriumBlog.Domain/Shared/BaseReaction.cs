using BisleriumBlog.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Domain.Shared
{
    public abstract class BaseReaction
    {
        public int UserId { get; set; }
        public ReactionType Type { get; set; }
        public DateTime ReactedAt { get; set; }
    }
}
