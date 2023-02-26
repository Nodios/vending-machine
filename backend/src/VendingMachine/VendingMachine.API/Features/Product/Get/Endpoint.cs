using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using VendingMachine.DataAccess;

namespace Product.Get
{
    public class Endpoint : Endpoint<Request, Response>
    {
        #region Fields

        private readonly VendingMachineDbContext _dbContext;

        #endregion Fields

        #region Constructors

        public Endpoint(VendingMachineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion Constructors

        #region Methods

        public override void Configure()
        {
            Get("products/{Id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var product = await _dbContext.Products.AsNoTracking().Select(p => new
            {
                p.Id,
                p.DateCreated,
                p.AmountAvailable,
                p.Cost,
                p.Name,
                p.SellerId,
                SellerName = p.Seller.UserName,
            }).FirstOrDefaultAsync(p => p.Id == r.Id);

            if (product == null)
            {
                await SendNotFoundAsync();
                return;
            }

            Response.Id = product.Id;
            Response.Name = product.Name;
            Response.AmountAvailable = product.AmountAvailable;
            Response.Cost = product.Cost;
            Response.SellerId = product.SellerId;
            Response.SellerName = product.SellerName;

            await SendAsync(Response);
        }

        #endregion Methods
    }
}