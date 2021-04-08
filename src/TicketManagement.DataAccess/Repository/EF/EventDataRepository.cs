using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

namespace TicketManagement.DataAccess.Repository.EF
{
    public class EventDataRepository : IRepository<EventData>, IEventDataRepositoryExtension
    {
        private readonly DataContext _dbcontext;

        public EventDataRepository(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public int Insert(EventData obj)
        {
            _dbcontext.Events.Add(obj);
            _dbcontext.SaveChanges();
            return obj.Id;
        }

        public int Delete(int id)
        {
            var del = _dbcontext.Events.Find(id);
            _dbcontext.Events.Remove(del);
            return _dbcontext.SaveChanges();
        }

        public List<EventData> FilterByVenueId(int input)
        {
            var eves = from ev in _dbcontext.Events
                       join lay in _dbcontext.Layouts on ev.LayoutId equals lay.Id
                       where lay.VenueId == input
                       select ev;
            return eves.ToList();
        }

        public EventData GetById(int id)
        {
            return _dbcontext.Events.Find(id);
        }

        public IQueryable<EventData> GetAll()
        {
            return _dbcontext.Events;
        }

        public int Update(EventData obj)
        {
            _dbcontext.Entry(obj).State= EntityState.Modified;
            _dbcontext.SaveChanges();
            return obj.Id;
        }
    }
}
