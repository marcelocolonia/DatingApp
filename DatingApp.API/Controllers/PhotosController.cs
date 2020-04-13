using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using DatingApp.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private readonly Cloudinary _cloudinary;

        public PhotosController(
            IUserRepository userRepository,
            IPhotoRepository photoRepository,
            IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._userRepository = userRepository;
            this._photoRepository = photoRepository;
            this._mapper = mapper;
            this._cloudinaryConfig = cloudinaryConfig;

            var account = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            this._cloudinary = new Cloudinary(account);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoDb = await _photoRepository.Get(id);

            var photoDto = _mapper.Map<PhotoForReturnDto>(photoDb);

            return Ok(photoDto);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> Add([FromForm] PhotoForCreationDto photoForCreationDto)
        {
            var userDb = await _userRepository.Get(HttpContext.LoggedUserId());

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            //  todo: move this to a customattribute validator
            if (file.Length > 0)
            {
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
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photoDb =_mapper.Map<Photo>(photoForCreationDto);

            if (!userDb.Photos.Any(photo => photo.IsMain))
            {
                photoDb.IsMain = true;
            }

            userDb.Photos.Add(photoDb);

            if (await _userRepository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photoDb);
                return CreatedAtRoute("GetPhoto", new { id = photoDb.Id, userId = HttpContext.LoggedUserId() }, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }
    }
}