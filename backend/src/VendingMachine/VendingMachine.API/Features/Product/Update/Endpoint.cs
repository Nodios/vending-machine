using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendingMachine.DataAccess;

namespace Product.Update
{
    public class Endpoint : Endpoint<Request, EmptyResponse>
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
            Put("products/{Id}");
            Claims(ClaimTypes.NameIdentifier);
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.SellerId == r.SellerId && p.Id == r.Id);

            if (product == null)
            {
                Logger.LogError($"User {r.SellerId} tried and failed to update product {r.Id}.");
                AddError("Unknown product.");
                await SendErrorsAsync();
                return;
            }

            product.Name = r.Name;
            product.AmountAvailable = r.AmountAvailable;
            product.Cost = r.Cost;

            product = VendingMachine.Domain.Product.Update(product);

            var errorMessage = $"Failed to update product {r.Id}";

            try
            {
                var result = await _dbContext.SaveChangesAsync();

                if (result == 1)
                {
                    await SendOkAsync();
                    return;
                }

                AddError(errorMessage);
                await SendErrorsAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                AddError(errorMessage);
                await SendErrorsAsync();
            }
        }

        #endregion Methods
    }
}