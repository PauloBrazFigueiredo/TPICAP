using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TPICAP.API.Interfaces;
using TPICAP.API.Models;

namespace TPICAP.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService()
        { 
        }

        public Task<string> CreateJwtSecurityToken(LoginModel login)
        {
            if(this.IsAuthenticated(login))
            {
                string jwtSecret = "1234567890123456";
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                            new Claim("UserName", login.UserName)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Task.FromResult(token);
            }
            return null;
        }

        private bool IsAuthenticated(LoginModel login)
        {
            return true;
        }
    }
}
