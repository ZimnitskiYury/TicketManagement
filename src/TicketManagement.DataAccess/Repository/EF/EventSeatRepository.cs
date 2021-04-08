using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repository.EF
{
    public class EventSeatRepository : IRepository<EventSeat>
    {
        private readonly DataContext _dbcontext;

        public EventSeatRepository(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public int Insert(EventSeat obj)
        {
            _dbcontext.EventSeats.Add(obj);
            _dbcontext.SaveChanges();
            return obj.Id;
        }

        public int Delete(int id)
        {
            var del = _dbcontext.EventSeats.Find(id);
            _dbcontext.EventSeats.Remove(del);
            return _dbcontext.SaveChanges();
        }

        public EventSeat GetById(int id)
        {
            return _dbcontext.EventSeats.Find(id);
        }

        public IQueryable<EventSeat> GetAll()
        {
            return _dbcontext.EventSeats;
        }

        public int Update(EventSeat obj)
        {
            _dbcontext.Entry(obj).State= EntityState.Modified;
            _dbcontext.SaveChanges();
            return obj.Id;
        }
    }
}
