using VendingMachine.Domain.Identity;

namespace VendingMachine.Domain
{
    public class Product
    {
        #region Properties

        public int AmountAvailable { get; set; }
        public decimal Cost { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public User Seller { get; set; }
        public string SellerId { get; set; }

        #endregion Properties

        #region Methods

        public static Product Create(Product product)
        {
            product.DateCreated = DateTime.UtcNow;
            product.DateUpdated = DateTime.UtcNow;
            product.Id = Guid.NewGuid();

            return product;
        }

        public static Product Update(Product product)
        {
            product.DateUpdated = DateTime.UtcNow;

            return product;
        }

        #endregion Methods
    }
}