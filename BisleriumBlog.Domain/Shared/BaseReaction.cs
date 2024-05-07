using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Domain.Enums;

namespace BisleriumBlog.Domain.Shared
{
    public abstract class BaseReaction
    {
        public string? UserId { get; set; }
        public ReactionType Type { get; set; }
        public DateTime ReactedAt { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
