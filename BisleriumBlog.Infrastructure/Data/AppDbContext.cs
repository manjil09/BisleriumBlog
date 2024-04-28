using BisleriumBlog.Domain.Entities;
using BisleriumBlog.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BisleriumBlog.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BlogReaction> BlogReactions { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
            var user = new IdentityUser
            {
                Id = ADMIN_ID,
                Email = "manjil.koju.a@gmail.com",
                EmailConfirmed = true,
                UserName = "SpAdmin",
                NormalizedUserName = "SPADMIN"
            };
            var passwordHasher = new PasswordHasher<IdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "admin_12345");
            builder.Entity<IdentityUser>().HasData(user);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_ID,
            });
        }
    }
}
