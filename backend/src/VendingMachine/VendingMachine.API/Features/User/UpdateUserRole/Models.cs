using FastEndpoints;
using FluentValidation;

namespace User.UpdateUserRole
{
    public class Request
    {
        #region Properties

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