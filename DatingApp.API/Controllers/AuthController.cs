using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DatingApp.Dtos;
using DatingApp.API.DatingApp.Utils;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly ISigningService _signing;

        public AuthController(
            IAuthRepository repository,
            IConfiguration configuration,
            ISigningService signing)
        {
            _repository = repository;
            _configuration = configuration;
            _signing = signing;
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
    }
}