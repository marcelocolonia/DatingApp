using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {

        private readonly IAuthRepository _repository;

        public AuthController(IAuthRepository repository)
        {
            _repository = repository;            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserToRegister userToRegister) {
            
            userToRegister.Username = userToRegister.Username.ToLower();

            if (await _repository.UserExists(userToRegister.Username))
                return BadRequest("User already exists");

            var userToCreate = new User {
                Username = userToRegister.Username
            };
            
            var createdUser = await _repository.Register(userToCreate, userToRegister.Password);

            return StatusCode(201);
        }
    }
}