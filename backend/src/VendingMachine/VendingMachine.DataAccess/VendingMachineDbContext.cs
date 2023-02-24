using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VendingMachine.Domain;
using VendingMachine.Domain.Configuration;
using VendingMachine.Domain.Identity;

namespace VendingMachine.DataAccess
{
    public class VendingMachineDbContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        #region Constructors

        public VendingMachineDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion Constructors

        #region Properties

        public DbSet<Product> Products { get; set; }

        #endregion Properties

        #region Methods

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            IdentityConfiguration.ConfigureIdentity(builder);
            IdentityConfiguration.SeedIdentity(builder);

            ProductConfiguration.Configure(builder);
        }

        #endregion Methods
    }
}