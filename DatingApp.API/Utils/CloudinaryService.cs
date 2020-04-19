using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Utils
{
    public class CloudinaryService : IPhotoUploadService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<PhotoUploadSettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            this._cloudinary = new Cloudinary(account);
        }

        public async Task<PhotoUploadResult> Upload(IFormFile file)
        {
            if (!(file.Length > 0))
                throw new System.Exception("File is empty");

            var uploadResult = new ImageUploadResult();

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation()
                    .Width(500)
                    .Height(500)
                    .Crop("fill")
                    .Gravity("face")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            return new PhotoUploadResult()
            {
                Url = uploadResult.Uri.ToString(),
                PublicId = uploadResult.PublicId
            };

        }

        public async Task<PhotoDeleteResult> Delete(string publicId)
        {
            var result = await this._cloudinary.DeleteResourcesAsync(publicId);

            return new PhotoDeleteResult {   
                StatusCode = result.StatusCode,
                Message = result.Error?.Message
            };
        }
    }
}