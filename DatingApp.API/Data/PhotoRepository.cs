using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DataContext context) : base(context) { }

        public override Task<Photo> Get(int id)
        {
            return this.Context.Photos.FirstOrDefaultAsync(photo => photo.Id == id);
        }

        public override IQueryable<Photo> GetAll()
        {
            return this.Context.Photos.AsQueryable();
        }
    }
}