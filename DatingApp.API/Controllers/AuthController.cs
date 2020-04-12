using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using DatingApp.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository _repository;
        private readonly IUserRepository _usersRepository;
        private readonly IConfiguration _configuration;
        private readonly ISigningService _signing;
        private readonly IMapper _mapper;

        public AuthController(
            IAuthRepository repository,
            IUserRepository usersRepository,
            IConfiguration configuration,
            ISigningService signing,
            IMapper mapper)
        {
            _repository = repository;
            _usersRepository = usersRepository;
            _configuration = configuration;
            _signing = signing;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegisterDto userToRegister)
        {

            userToRegister.Username = userToRegister.Username.ToLower();

            if (await _repository.UserExists(userToRegister.Username))
                return BadRequest("User already exists");

            var userToCreate = new User
            {
                Username = userToRegister.Username
            };

            var createdUser = await _repository.Register(userToCreate, userToRegister.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserToLoginDto userToLogin)
        {

            var userInRepository = await _repository.Login(userToLogin.Username, userToLogin.Password);

            if (userInRepository == null)
                return Unauthorized();

            var token = _signing.GenerateToken(new string[] {
                userInRepository.Id.ToString(),
                userInRepository.Username,
            });

            return Ok(new
            {
                token = token
            });

        }

        [Authorize]
        [HttpPost("updateProfile")]
        public async Task<IActionResult> Post(UserForUpdateDto model)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //  todo: dont like the array thing
            var userDb = await _usersRepository.Get(int.Parse(userId));

            _mapper.Map(model, userDb);

            if (await _usersRepository.SaveAll())
                return NoContent();

            return BadRequest("Error when updating user profile");
        }
    }
}