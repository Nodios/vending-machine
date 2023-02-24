using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using VendingMachine.API.Infrastructure;
using VendingMachine.API.Infrastructure.Parameters;
using VendingMachine.DataAccess;

namespace Product.Find
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
            Get("products");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var query = _dbContext.Products.AsNoTracking().AsQueryable();

            var totalItems = await query.CountAsync();

            if (!string.IsNullOrWhiteSpace(r.SellerId) && Guid.TryParse(r.SellerId, out Guid _))
            {
                query = query.Where(p => p.SellerId == r.SellerId);
            }

            var sortParameter = SortParameter.FromString(r.Sort);
            query = (sortParameter.OrderBy switch
            {
                "amountAvailable" => query.Order(p => p.AmountAvailable, sortParameter.Ascending),
                "cost" => query.Order(p => p.Cost, sortParameter.Ascending),
                "name" => query.Order(p => p.Name, sortParameter.Ascending),
                _ => query.Order(p => p.DateCreated, sortParameter.Ascending),
            }).ThenBy(p => p.Id);

            query = query.Skip(r.Skip ?? 0).Take(r.Take ?? 10);

            var items = await query.Select(p => new
            {
                p.Id,
                p.DateCreated,
                p.AmountAvailable,
                p.Cost,
                p.Name,
                p.SellerId,
                SellerName = p.Seller.UserName,
            }).ToListAsync();

            Response.TotalItems = totalItems;
            Response.Products = items.Select(p => new Product
            {
                AmountAvailable = p.AmountAvailable,
                Cost = p.Cost,
                Name = p.Name,
                Id = p.Id,
                SellerId = p.SellerId,
                SellerName = p.SellerName,
            });

            await SendAsync(Response);
        }

        #endregion Methods
    }
}