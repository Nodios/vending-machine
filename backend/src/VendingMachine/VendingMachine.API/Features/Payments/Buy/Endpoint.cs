using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendingMachine.API.Infrastructure;
using VendingMachine.DataAccess;
using VendingMachine.Domain.Configuration;

namespace Payments.Buy
{
    public class Endpoint : Endpoint<Request, Response>
    {
        #region Fields

        private readonly CoinChanger _coinChanger;
        private readonly VendingMachineDbContext _dbContext;
        private readonly UserManager<VendingMachine.Domain.Identity.User> _userManager;

        #endregion Fields

        #region Constructors

        public Endpoint(UserManager<VendingMachine.Domain.Identity.User> userManager, VendingMachineDbContext dbContext, CoinChanger coinChanger)
        {
            _dbContext = dbContext;
            _coinChanger = coinChanger;
            _userManager = userManager;
        }

        #endregion Constructors

        #region Methods

        public override void Configure()
        {
            Post("payments/buy");
            Roles(RoleNames.BUYER);
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var user = await _userManager.FindByIdAsync(r.UserId);

            if (user == null)
            {
                AddError("Unknown user.");
                await SendErrorsAsync();
                return;
            }

            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == r.ProductId);

            if (product == null)
            {
                Logger.LogWarning($"User {r.UserId} tried to buy unknown product {r.ProductId}.");
                AddError("Unknown product.");
                await SendErrorsAsync();
                return;
            }

            if (product.AmountAvailable == 0)
            {
                Logger.LogWarning($"User {r.UserId} tried to buy unavailable product {r.ProductId}.");
                AddError("Product is not in stock");
                await SendErrorsAsync();
                return;
            }

            var totalCost = product.Cost * r.Quantity;

            if (user.Deposit < totalCost)
            {
                Logger.LogWarning($"User {r.UserId} has insufficient funds to buy {r.Quantity} of product {r.ProductId}.");
                AddError($"Insufficient funds - required amount is {totalCost}");
                await SendErrorsAsync();
                return;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    product.AmountAvailable -= r.Quantity;
                    await _dbContext.SaveChangesAsync();

                    var denominations = Config.GetSection("Settings:AcceptedCoins").Get<int[]>() ?? Array.Empty<int>();
                    // TODO: change to decimal
                    var coinChangeResult = _coinChanger.GetChange(denominations, (int)(user.Deposit - totalCost));
                    user.Deposit = coinChangeResult.Left;

                    await _userManager.UpdateAsync(user);

                    await transaction.CommitAsync();

                    Response.Change = coinChangeResult.Change;
                    Response.Spent = totalCost;
                    Response.AvailableFunds = user.Deposit;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, ex.Message);

                    AddError("Something went wrong while purchasing.");
                    await SendErrorsAsync();
                    return;
                }
            }

            await SendAsync(Response);
        }

        #endregion Methods
    }
}