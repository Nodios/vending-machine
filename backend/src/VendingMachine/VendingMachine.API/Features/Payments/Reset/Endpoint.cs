using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using VendingMachine.Domain.Configuration;

namespace Payments.Reset
{
    public class Endpoint : Endpoint<Request, Response>
    {
        #region Fields

        private readonly UserManager<VendingMachine.Domain.Identity.User> _userManager;

        #endregion Fields

        #region Constructors

        public Endpoint(UserManager<VendingMachine.Domain.Identity.User> userManager)
        {
            _userManager = userManager;
        }

        #endregion Constructors

        #region Methods

        public override void Configure()
        {
            Delete("payments/reset");
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

            user.Deposit = 0;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                foreach (var err in updateResult.Errors)
                {
                    AddError(err.Description, err.Code);
                }
                await SendErrorsAsync();
                return;
            }

            Response.AvailableFunds = 0;
            Logger.LogInformation($"User {r.UserId} reset their deposit.");

            await SendAsync(Response);
        }

        #endregion Methods
    }
}