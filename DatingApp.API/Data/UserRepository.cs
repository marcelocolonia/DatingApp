using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) {}

        public async override Task<User> Get(int id)
        {
            var user = await this.Context.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == id);

            return user;
        }

        public override IQueryable<User> GetAll()
        {
            return this.Context.Users.Include(x => x.Photos).AsQueryable();
        }
    }
}