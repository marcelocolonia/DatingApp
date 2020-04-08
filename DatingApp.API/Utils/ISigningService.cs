using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Utils
{
    public interface ISigningService
    {
        SecurityKey SecurityKey { get; }

        string GenerateToken(string[] claims);
    }
}