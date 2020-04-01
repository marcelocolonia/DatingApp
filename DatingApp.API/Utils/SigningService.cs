using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.DatingApp.Utils
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
    }
}