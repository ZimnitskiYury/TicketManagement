using System.Linq;

namespace TicketManagement.DataAccess.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        T GetById(int id);

        int Insert(T obj);

        int Update(T obj);

        IQueryable<T> GetAll();

        int Delete(int id);
    }
}
