using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BisleriumBlog.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BlogReaction> BlogReactions { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }
        public DbSet<BlogHistory> BlogHistory { get; set; }
        public DbSet<CommentHistory> CommentHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Blog>()
            //    .HasOne(b => b.User)
            //    .WithMany(u => u.Blogs) 
            //    .HasForeignKey<Blog>(b => b.UserId)
            //    .OnDelete(DeleteBehavior.NoAction) 
            //    .onUpdate(DeleteBehavior.NoAction);

            //make sure each user can only have 1 reaction and 1 comment on each blog
            builder.Entity<Comment>().HasIndex(x => new { x.BlogId, x.UserId }).IsUnique();
            builder.Entity<BlogReaction>().HasIndex(x => new { x.BlogId, x.UserId }).IsUnique();
            builder.Entity<CommentReaction>().HasIndex(x => new { x.CommentId, x.UserId }).IsUnique();

            string ADMIN_ID = "36109bb5-c596-4fee-a016-1d8f8c7496cd";
            string ADMIN_ROLE_ID = "3e4007fd-f323-41c5-892a-29e4c50d0f6b";
            string USER_ROLE_ID = "f8ca833b-8b85-41b3-9279-eb99b563326d";

            //seed admin role
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = UserRole.Admin.ToString(),
                NormalizedName = UserRole.Admin.ToString().ToUpper(),
                Id = ADMIN_ROLE_ID,
                ConcurrencyStamp = ADMIN_ROLE_ID,
            });

            //seed user role
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = UserRole.User.ToString(),
                NormalizedName = UserRole.User.ToString().ToUpper(),
                Id = USER_ROLE_ID,
                ConcurrencyStamp = USER_ROLE_ID,
            });

            //seed user
            var user = new ApplicationUser
            {
                Id = ADMIN_ID,
                Email = "manjil.koju.a@gmail.com",
                EmailConfirmed = true,
                UserName = "SpAdmin",
                NormalizedUserName = "SPADMIN"
            };
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "admin_12345");
            builder.Entity<ApplicationUser>().HasData(user);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_ID,
            });
        }
    }
}
