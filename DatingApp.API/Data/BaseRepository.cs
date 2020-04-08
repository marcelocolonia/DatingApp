using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;

namespace DatingApp.API.Data
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T: class
    {
        private readonly DataContext _context;

        public DataContext Context => _context;

        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            var changes = await _context.SaveChangesAsync();
            return changes > 0;
        }

        public abstract Task<T> Get(int id);
        public abstract IQueryable<T> GetAll();
    }
}