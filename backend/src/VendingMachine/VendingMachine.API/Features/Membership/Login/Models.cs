using FastEndpoints;
using FluentValidation;

namespace Membership.Login
{
    public class Request
    {
        #region Properties

        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string Username { get; set; }

        #endregion Properties
    }

    public class Response
    {
        #region Properties

        public decimal AvailableFunds { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Id { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Token { get; set; }
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
            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("Password is required.");
        }

        #endregion Constructors
    }
}