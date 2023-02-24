using FastEndpoints;
using FluentValidation;
using System.Security.Claims;

namespace Product.Create
{
    public class Request
    {
        #region Properties

        public int AmountAvailable { get; set; }

        // TODO: put back as decimal
        public int Cost { get; set; }

        public string Name { get; set; }

        [FromClaim(ClaimType = ClaimTypes.NameIdentifier, IsRequired = true)]
        public string SellerId { get; set; }

        #endregion Properties
    }

    public class Response
    {
        #region Properties

        public int AmountAvailable { get; set; }

        // TODO: put back as decimal
        public int Cost { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }

        #endregion Properties
    }

    public class Validator : Validator<Request>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(p => p.AmountAvailable).GreaterThanOrEqualTo(0).WithMessage("Amount available must be 0 or more.");
            RuleFor(p => p.Cost).GreaterThan(0).WithMessage("Cost must be more than 0.");
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required.");
        }

        #endregion Constructors
    }
}