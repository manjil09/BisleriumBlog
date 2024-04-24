using BisleriumBlog.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace BisleriumBlog.Domain.Shared
{
    public abstract class BaseReaction
    {
        public ReactionType Type { get; set; }
        public DateTime ReactedAt { get; set; }
    }
}
