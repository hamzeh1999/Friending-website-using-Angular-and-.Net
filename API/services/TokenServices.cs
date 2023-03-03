using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.entities;
using API.interfaces;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Identity;
// using Microsoft.IdentityModel.JsonWebTokens;

namespace API.services
{
    public class TokenServices : ITokenServices
    {
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
        public TokenServices(IConfiguration config, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),

        };

            var roles= await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role=>new Claim(ClaimTypes.Role,role)));
            
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);


        }
    }
}