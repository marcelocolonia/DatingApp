using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repository;

        public UsersController(IUserRepository repository)
        {
            _repository = repository;            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repository.Get(id);

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _repository.GetAll().ToListAsync();

            return Ok(users);
        }
    }
}