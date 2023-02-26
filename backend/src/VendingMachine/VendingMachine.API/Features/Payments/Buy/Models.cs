using FastEndpoints;
using FluentValidation;
using System.Security.Claims;

namespace Payments.Buy
{
    public class Request
    {
        #region Properties

        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        [FromClaim(ClaimType = ClaimTypes.NameIdentifier, IsRequired = true)]
        public string UserId { get; set; }

        #endregion Properties
    }

    public class Response
    {
        #region Properties

        public decimal AvailableFunds { get; set; }
        public IEnumerable<int> Change { get; set; }
        public decimal Spent { get; set; }

        #endregion Properties
    }

    public class Validator : Validator<Request>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(p => p.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.");
            RuleFor(p => p.Quantity)
                .NotEmpty()
                .WithMessage("ProductId is required.")
                .GreaterThanOrEqualTo(1)
                .WithMessage("At least one product must be added.");
        }

        #endregion Constructors
    }
}