using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data.Interfaces
{
    public interface IBaseRepository<T> where T: class
    {
        void Add(T entity);
        void Delete(T entity);
        Task<bool> SaveAll();

        Task<T> Get(int id);
        IQueryable<T> GetAll();
    }
}