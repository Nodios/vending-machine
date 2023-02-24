using Microsoft.EntityFrameworkCore;

namespace VendingMachine.Domain.Configuration
{
    public class ProductConfiguration
    {
        #region Methods

        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<Product>().ToTable("products");
            builder.Entity<Product>().HasKey(p => p.Id);

            builder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.SellerId);

            builder.Entity<Product>().Property(p => p.Cost).IsRequired();
            builder.Entity<Product>().Property(p => p.Name).IsRequired();
            builder.Entity<Product>().Property(p => p.AmountAvailable).IsRequired();
        }

        #endregion Methods
    }
}