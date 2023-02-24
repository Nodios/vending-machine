using FastEndpoints;
using System.Security.Claims;

namespace User.Update
{
    public class Request
    {
        #region Properties

        public string Email { get; set; }

        [FromClaim(ClaimType = ClaimTypes.NameIdentifier, IsRequired = true)]
        public string UserId { get; set; }

        #endregion Properties
    }

    public class Validator : Validator<Request>
    {
        #region Constructors

        public Validator()
        {
        }

        #endregion Constructors
    }
}