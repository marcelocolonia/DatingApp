using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IPhotoUploadService _photoUploadService;
        private readonly IMapper _mapper;

        public PhotosController(
            IUserRepository userRepository,
            IPhotoRepository photoRepository,
            IPhotoUploadService photoUploadService,
            IMapper mapper)
        {
            this._userRepository = userRepository;
            this._photoRepository = photoRepository;
            this._photoUploadService = photoUploadService;
            this._mapper = mapper;
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

            var uploadResult = await _photoUploadService.Upload(photoForCreationDto.File);

            _mapper.Map(uploadResult, photoForCreationDto);

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