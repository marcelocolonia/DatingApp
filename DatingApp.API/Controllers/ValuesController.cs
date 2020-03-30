using DatingApp.API.DatingApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValueController : ControllerBase
    {
        public readonly DataContext _context;

        public ValueController(DataContext context)
        {
            this._context = context;            
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var values = await this._context.Values.ToListAsync();

            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var value = await this._context.Values.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(value);
        }
    }
}
