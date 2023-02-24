using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using VendingMachine.Domain.Configuration;

namespace User.Register
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
            Post("users");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var existingUser = await _userManager.FindByNameAsync(r.Username);

            if (existingUser != null)
            {
                AddError("Username is taken.");
                await SendErrorsAsync();
                return;
            }

            existingUser = await _userManager.FindByEmailAsync(r.Email);

            if (existingUser != null)
            {
                AddError("User with this email already exists.");
                await SendErrorsAsync();
                return;
            }

            var userId = Guid.NewGuid().ToString();
            var user = new VendingMachine.Domain.Identity.User
            {
                Id = userId,
                Email = r.Email,
                UserName = r.Username,
                EmailConfirmed = true,
                Deposit = 0,
            };

            var result = await _userManager.CreateAsync(user, r.Password);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    AddError(err.Description, err.Code);
                }

                await SendErrorsAsync();
                return;
            }

            await _userManager.AddToRoleAsync(user, RoleNames.BUYER);

            Response.Id = userId;
            Response.Email = r.Email;
            Response.Username = r.Username;

            await SendAsync(Response);
        }

        #endregion Methods
    }
}