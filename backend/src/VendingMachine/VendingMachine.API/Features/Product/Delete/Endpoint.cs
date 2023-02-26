using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VendingMachine.DataAccess;

namespace Product.Delete
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
            Delete("products/{Id}");
            Claims(ClaimTypes.NameIdentifier);
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.SellerId == r.SellerId && p.Id == r.Id);

            if (product == null)
            {
                Logger.LogError($"User {r.SellerId} tried and failed to delete product {r.Id}.");
                AddError("Product can't be deleted.");
                await SendErrorsAsync();
                return;
            }

            _dbContext.Products.Remove(product);

            var errorMessage = $"Failed to delete product {r.Id}";

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