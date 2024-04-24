using BisleriumBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BisleriumBlog.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
    }
}
