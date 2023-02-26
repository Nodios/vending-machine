using FastEndpoints;
using FluentValidation;

namespace User.Find
{
    public class Request
    {
        #region Properties

        public int? Skip { get; set; }
        public int? Take { get; set; }

        #endregion Properties
    }

    public class Response
    {
        #region Properties

        public int TotalItems { get; set; }
        public IEnumerable<UserViewModel> Users { get; set; }

        #endregion Properties
    }

    public class UserViewModel
    {
        #region Properties

        public string Email { get; set; }
        public string Id { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Username { get; set; }

        #endregion Properties
    }

    public class Validator : Validator<Request>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(p => p.Skip)
                .GreaterThanOrEqualTo(0)
                .When(p => p.Skip.HasValue);
            RuleFor(p => p.Take)
                .GreaterThanOrEqualTo(1)
                .When(p => p.Take.HasValue)
                .LessThanOrEqualTo(100)
                .When(p => p.Take.HasValue);
        }

        #endregion Constructors
    }
}