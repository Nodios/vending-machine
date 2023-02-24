using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VendingMachine.API.Infrastructure;
using VendingMachine.Domain.Identity;

namespace Membership.Login
{
    public class Endpoint : Endpoint<Request, Response>
    {
        #region Fields

        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<VendingMachine.Domain.Identity.User> _userManager;

        #endregion Fields

        #region Constructors

        public Endpoint(
            RoleManager<Role> roleManager,
            UserManager<VendingMachine.Domain.Identity.User> userManager
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #endregion Constructors

        #region Methods

        public override void Configure()
        {
            Post("login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request r, CancellationToken c)
        {
            var user = await _userManager.FindByNameAsync(r.Username);

            if (user == null)
            {
                AddError("Unknown user or password");
                await SendErrorsAsync();
                return;
            }

            bool validPassword = await _userManager.CheckPasswordAsync(user, r.Password);
            if (!validPassword)
            {
                AddError("Unknown user or password");
                await SendErrorsAsync();
                return;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim("role", role));
            }

            // GENERATE TOKEN
            DateTime expiry = r.RememberMe ? DateTime.UtcNow.AddMonths(6) : DateTime.UtcNow.AddHours(2);

            var token = TokenGenerator.GenerateToken(Config, claims, expiry);

            // TODO: add token persistence

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            Response.Id = user.Id;
            Response.Email = user.Email;
            Response.Username = user.UserName;
            Response.Token = accessToken;
            Response.ExpiresAt = expiry;
            Response.Roles = userRoles;
            Response.AvailableFunds = user.Deposit;

            await SendAsync(Response);
        }

        #endregion Methods
    }
}