using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.DatingApp.Utils
{
    public interface ISigningService
    {
        SecurityKey SecurityKey { get; }
    }
}