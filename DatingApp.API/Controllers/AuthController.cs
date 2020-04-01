using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DatingApp.Dtos;
using DatingApp.API.DatingApp.Utils;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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

            //  creating payload
            var claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, userInRepository.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userInRepository.Username),
            };

            var signingCredentials = new SigningCredentials(_signing.SecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials,
                Expires = DateTime.UtcNow.AddDays(1),
            };

            var jwtHandler = new JwtSecurityTokenHandler();

            var token = jwtHandler.CreateToken(securityTokenDescriptor);

            return Ok(new
            {
                token = jwtHandler.WriteToken(token)
            });

        }
    }
}