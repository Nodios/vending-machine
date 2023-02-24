using FastEndpoints;
using FluentValidation;

namespace Product.Find
{
    public class Product
    {
        #region Properties

        public int AmountAvailable { get; set; }
        public decimal Cost { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SellerId { get; set; }
        public string SellerName { get; set; }

        #endregion Properties
    }

    public class Request
    {
        #region Properties

        public string SellerId { get; set; }
        public int? Skip { get; set; }
        public string Sort { get; set; }
        public int? Take { get; set; }

        #endregion Properties
    }

    public class Response
    {
        #region Properties

        public IEnumerable<Product> Products { get; set; }
        public int TotalItems { get; set; }

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