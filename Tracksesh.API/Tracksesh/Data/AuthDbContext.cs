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

            var readerRoleId = "cd70dfdc-529c-45b4-9020-8d51146118a6";
            var writerRoleId = "f0903c93-d5d1-4de7-af71-9431bb8953ef";

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

            var adminUserId = "2ee12279-8dcd-46b8-b97c-d3096da594f4";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@tracksesh.com",
                Email = "admin@tracksesh.com",
                NormalizedEmail = "admin@tracksesh.com".ToUpper(),
                NormalizedUserName = "admin@tracksesh.com".ToUpper(),
                PasswordHash = "AQAAAAIAAYagAAAAEGiToLd6AQjHzQ++RZ3u5vxeDF/CazHXaiofx37um3TxFuHQc93iJsxGRbP05/1l/Q==",
                SecurityStamp = "8a99d7e7-a923-4d16-82ea-43df081b7511",
                ConcurrencyStamp = "9d7ecfb5-ff6b-4a54-91ce-60fb9383def8",
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
