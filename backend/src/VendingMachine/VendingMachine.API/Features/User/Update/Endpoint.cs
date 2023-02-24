using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace User.Update
{
    public class Endpoint : Endpoint<Request, EmptyRequest>
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
            Put("user");
            ClaimsAll(ClaimTypes.NameIdentifier, ClaimTypes.Name);
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

            user.Email = r.Email;

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

            Logger.LogInformation($"Updated user {r.UserId}");
            await SendOkAsync();
        }

        #endregion Methods
    }
}