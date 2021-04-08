using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repository.EF
{
    public class EventAreaRepository : IRepository<EventArea>
    {
        private readonly DataContext _dbcontext;

        public EventAreaRepository(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public int Insert(EventArea obj)
        {
            _dbcontext.EventAreas.Add(obj);
            _dbcontext.SaveChanges();
            return obj.Id;
        }

        public int Delete(int id)
        {
            var del = _dbcontext.EventAreas.Find(id);
            _dbcontext.EventAreas.Remove(del);
            return _dbcontext.SaveChanges();
        }

        public EventArea GetById(int id)
        {
            return _dbcontext.EventAreas.Find(id);
        }

        public IQueryable<EventArea> GetAll()
        {
            return _dbcontext.EventAreas;
        }

        public int Update(EventArea obj)
        {
            _dbcontext.Entry(obj).State = EntityState.Modified;
            _dbcontext.SaveChanges();
            return obj.Id;
        }
    }
}
