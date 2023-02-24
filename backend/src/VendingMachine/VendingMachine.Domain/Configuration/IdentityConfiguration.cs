using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendingMachine.Domain.Identity;

namespace VendingMachine.Domain.Configuration
{
    public class IdentityConfiguration
    {
        #region Methods

        public static void ConfigureIdentity(ModelBuilder builder)
        {
            builder.Entity<User>(entity => { entity.ToTable(name: "users"); });
            builder.Entity<Role>(entity => { entity.ToTable(name: "roles"); });
            builder.Entity<UserRole>(entity => { entity.ToTable("user_roles"); });
            builder.Entity<UserClaim>(entity => { entity.ToTable("user_claims"); });
            builder.Entity<UserLogin>(entity => { entity.ToTable("user_logins"); });
            builder.Entity<UserToken>(entity => { entity.ToTable("user_tokens"); });
            builder.Entity<RoleClaim>(entity => { entity.ToTable("role_claims"); });
        }

        public static void SeedIdentity(ModelBuilder builder)
        {
            // SEED ADMIN USER
            User user = new()
            {
                Id = "30A07246-8B22-4F6C-B41B-408A9D0ADAFE",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@vending-machines.dev",
                NormalizedEmail = "ADMIN@VENDING-MACHINES.DEV",
                LockoutEnabled = false,
                SecurityStamp = "CF5C6F0F-D4A6-4A2B-97B8-9C4E11905B3C"
            };

            PasswordHasher<User> passwordHasher = new();
            user.PasswordHash = passwordHasher.HashPassword(user, "Adm1n");

            builder.Entity<User>().HasData(user);

            // SEED ROLES
            builder.Entity<Role>().HasData(
                    new Role
                    {
                        Id = "5511CA1E-79D5-4CC9-A23A-83A4FB08C885",
                        Name = RoleNames.ADMIN,
                        NormalizedName = RoleNames.ADMIN.ToUpperInvariant(),
                        ConcurrencyStamp = "CF5C6F0F-D4A6-4A2B-97B8-9C4E11905B3C"
                    },
                    new Role
                    {
                        Id = "816AFB1C-FC88-4727-A663-12FB0983728E",
                        Name = RoleNames.SELLER,
                        NormalizedName = RoleNames.SELLER.ToUpperInvariant(),
                        ConcurrencyStamp = "0546E885-5976-4FF1-ADA7-CDECE5A8D005"
                    },
                    new Role
                    {
                        Id = "35C13B55-E64E-49BE-92FE-91A3846091CB",
                        Name = RoleNames.BUYER,
                        NormalizedName = RoleNames.BUYER.ToUpperInvariant(),
                        ConcurrencyStamp = "A2A48471-D004-4E6D-ACD8-571F7F5531E6"
                    }
                );

            // SEED USER ROLES - Admin only
            builder.Entity<UserRole>().HasData(
                    new UserRole
                    {
                        RoleId = "5511CA1E-79D5-4CC9-A23A-83A4FB08C885",
                        UserId = "30A07246-8B22-4F6C-B41B-408A9D0ADAFE"
                    }
                );
        }

        #endregion Methods
    }
}