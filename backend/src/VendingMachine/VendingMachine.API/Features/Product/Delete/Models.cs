using FastEndpoints;
using System.Security.Claims;

namespace Product.Delete
{
    public class Request
    {
        #region Properties

        public Guid Id { get; set; }

        [FromClaim(ClaimType = ClaimTypes.NameIdentifier, IsRequired = true)]
        public string SellerId { get; set; }

        #endregion Properties
    }
}