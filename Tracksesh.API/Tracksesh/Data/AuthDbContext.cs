using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Tracksesh.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "";
            var writerRoleId = "";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            var adminUserId = "";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@tracksesh.com",
                Email = "admin@tracksesh.com",
                NormalizedEmail = "admin@tracksesh.com".ToUpper(),
                NormalizedUserName = "admin@tracksesh.com".ToUpper(),
                PasswordHash = "",
                SecurityStamp = "",
                ConcurrencyStamp = "",
            };

            builder.Entity<IdentityUser>().HasData(admin);

            var adminRole = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = readerRoleId,
                    UserId = adminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = writerRoleId,
                    UserId = adminUserId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRole);
        }

    }

}
