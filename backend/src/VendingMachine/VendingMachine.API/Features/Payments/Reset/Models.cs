using FastEndpoints;
using System.Security.Claims;

namespace Payments.Reset
{
    public class Request
    {
        #region Properties

        [FromClaim(ClaimType = ClaimTypes.NameIdentifier, IsRequired = true)]
        public string UserId { get; set; }

        #endregion Properties
    }

    public class Response
    {
        #region Properties

        public decimal AvailableFunds { get; set; }

        #endregion Properties
    }
}