using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using VendingMachine.Domain.Configuration;

namespace User.UpdateUserRole
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
            Verbs(Http.PUT, Http.DELETE);
            Routes("users/role");
            Roles(RoleNames.ADMIN);
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            if (r.UserId == r.RequestUserId)
            {
                AddError("Can not change role to yourself.");
                await SendErrorsAsync();
                return;
            }

            var user = await _userManager.FindByIdAsync(r.UserId);

            if (user == null)
            {
                AddError("Unknown user");
                await SendErrorsAsync();
                return;
            }

            var userInRole = await _userManager.IsInRoleAsync(user, r.Role);
            if (HttpMethod == Http.PUT)
            {
                if (userInRole)
                {
                    Logger.LogInformation($"User {r.UserId} is already in role {r.Role}.");
                    await SendOkAsync();
                    return;
                }

                var roleAddResult = await _userManager.AddToRoleAsync(user, r.Role);

                if (!roleAddResult.Succeeded)
                {
                    foreach (var err in roleAddResult.Errors)
                    {
                        AddError(err.Description, err.Code);
                    }
                    await SendErrorsAsync();
                    return;
                }
            }
            else if (HttpMethod == Http.DELETE)
            {
                if (!userInRole)
                {
                    Logger.LogInformation($"User {r.UserId} is not in role {r.Role}.");
                    await SendOkAsync();
                    return;
                }

                var roleRemoveResult = await _userManager.RemoveFromRoleAsync(user, r.Role);

                if (!roleRemoveResult.Succeeded)
                {
                    foreach (var err in roleRemoveResult.Errors)
                    {
                        AddError(err.Description, err.Code);
                    }
                    await SendErrorsAsync();
                    return;
                }
            }

            await SendOkAsync();
        }

        #endregion Methods
    }
}