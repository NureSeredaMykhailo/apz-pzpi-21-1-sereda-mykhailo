using EQueue.Db.Models;
using EQueue.Server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EQueue.Server.Services.Implementations
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;

        public JwtTokenGenerator(IConfiguration config, UserManager<User> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> GenerateAsync(User user)
        {
            var claims = GetUserClaims(user);
            var roleNames = await _userManager.GetRolesAsync(user);
            AddRoleClaims(claims, roleNames);

            var securityKey = _config.GetRequiredSection("SecurityKey").Value;

            var header = new JwtHeader(
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey)),
                    SecurityAlgorithms.HmacSha256));

            var token = new JwtSecurityToken(header, new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static List<Claim> GetUserClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf,
                    new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,
                    new DateTimeOffset(DateTime.Now.AddDays(30)).ToUnixTimeSeconds().ToString()),
            };
            return claims;
        }

        private static void AddRoleClaims(IList<Claim> claims, IList<string> roleNames)
        {
            foreach (var roleName in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }
        }
    }
}
