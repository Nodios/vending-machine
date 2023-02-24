using FastEndpoints;
using FluentValidation;

namespace User.Register
{
    public class Request
    {
        #region Properties

        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }

        #endregion Properties
    }

    public class Response
    {
        #region Properties

        public string Email { get; set; }
        public string Id { get; set; }
        public string Username { get; set; }

        #endregion Properties
    }

    public class Validator : Validator<Request>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(p => p.Username)
                .NotEmpty().WithMessage("Username is required.");
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Must be valid email.");
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.");
            RuleFor(p => p.ConfirmPassword)
                .Equal(p => p.Password).WithMessage("Password and Confirm password must be equal.");
        }

        #endregion Constructors
    }
}