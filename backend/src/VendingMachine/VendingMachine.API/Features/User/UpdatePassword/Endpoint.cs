using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace User.UpdatePassword
{
    public class Endpoint : Endpoint<Request, EmptyResponse>
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
            Put("user/password");
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

            var passwordChangeResult = await _userManager.ChangePasswordAsync(user, r.CurrentPassword, r.NewPassword);

            if (!passwordChangeResult.Succeeded)
            {
                foreach (var err in passwordChangeResult.Errors)
                {
                    AddError(err.Description, err.Code);
                }
                await SendErrorsAsync();
                return;
            }

            await SendOkAsync();
        }

        #endregion Methods
    }
}