using FastEndpoints;
using FluentValidation;
using System.Security.Claims;

namespace User.UpdateUserRole
{
    public class Request
    {
        #region Properties

        [FromClaim(ClaimType = ClaimTypes.NameIdentifier, IsRequired = true)]
        public string RequestUserId { get; set; }

        public string Role { get; set; }
        public string UserId { get; set; }

        #endregion Properties
    }

    public class Validator : Validator<Request>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(p => p.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(p => p.Role).NotEmpty().WithMessage("Role is required.");
        }

        #endregion Constructors
    }
}