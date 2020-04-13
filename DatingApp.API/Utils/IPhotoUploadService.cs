using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Utils
{
    public interface IPhotoUploadService
    {
        Task<PhotoUploadResult> Upload(IFormFile file);
    }
}
