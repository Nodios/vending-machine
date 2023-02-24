using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VendingMachine.API.Infrastructure
{
    public class TokenGenerator
    {
        #region Methods

        public static JwtSecurityToken GenerateToken(IConfiguration configuration, List<Claim> claims, DateTime expiry)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Auth:Secret")));
            JwtSecurityToken token = new JwtSecurityToken(
                expires: expiry,
                claims: claims,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        #endregion Methods
    }
}