using FastEndpoints;
using FluentValidation;
using System.Security.Claims;

namespace Payments.Deposit
{
    public class Request
    {
        #region Properties

        public IEnumerable<int> Coins { get; set; }

        [FromClaim(ClaimType = ClaimTypes.NameIdentifier, IsRequired = true)]
        public string UserId { get; set; }

        #endregion Properties
    }

    public class Response
    {
        #region Properties

        public int AmountAdded { get; set; }
        public decimal AvailableFunds { get; set; }

        #endregion Properties
    }

    public class Validator : Validator<Request>
    {
        #region Constructors

        public Validator(IConfiguration configuration)
        {
            RuleFor(p => p.Coins)
                .NotEmpty().WithMessage("Insert at least one coin.");

            var denominations = configuration.GetSection("Settings:AcceptedCoins").Get<int[]>() ?? Array.Empty<int>();
            RuleForEach(p => p.Coins)
                .Must(c => denominations.Contains(c))
                .WithMessage($"Invalid denomination. Accepted are: {string.Join(", ", denominations)}");
        }

        #endregion Constructors
    }
}