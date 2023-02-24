using Microsoft.AspNetCore.Identity;

namespace VendingMachine.Domain.Identity
{
    public class User : IdentityUser<string>
    {
        #region Properties

        public decimal Deposit { get; set; }

        public List<Product> Products { get; set; }

        #endregion Properties
    }
}