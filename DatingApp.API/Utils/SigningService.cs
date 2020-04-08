using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace DatingApp.API.Utils
{
    public class SigningService : ISigningService
    {
        public SecurityKey _securityKey;

        public SigningService(IConfiguration configuration)
        {
            _securityKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value)
            );

        }

        public SecurityKey SecurityKey => _securityKey;

        public string GenerateToken(string[] values)
        {
            //  turns the values into a list of claims
            var claims = values.Select(claim => new Claim(ClaimTypes.NameIdentifier, claim));

            //  setting up the credentials
            var signingCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha512Signature);

            //  generating the descriptor
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials,
                Expires = DateTime.UtcNow.AddDays(1),
            };

            var jwtHandler = new JwtSecurityTokenHandler();

            var token = jwtHandler.CreateToken(securityTokenDescriptor);

            return jwtHandler.WriteToken(token);
        }
    }
}