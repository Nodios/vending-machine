using FastEndpoints;
using FluentValidation;
using System.Security.Claims;

namespace User.UpdatePassword
{
    public class Request
    {
        #region Properties

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        [FromClaim(ClaimType = ClaimTypes.NameIdentifier, IsRequired = true)]
        public string UserId { get; set; }

        #endregion Properties
    }

    public class Validator : Validator<Request>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(p => p.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required.");
            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .NotEqual(p => p.CurrentPassword).WithMessage("New password must be different from the current password.");
        }

        #endregion Constructors
    }
}