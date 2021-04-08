using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

namespace TicketManagement.DataAccess.Repository.EF
{
    public class VenueRepository : IRepository<Venue>, IVenueRepositoryExtension
    {
        private readonly DataContext _dbcontext;

        public VenueRepository(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public int Insert(Venue obj)
        {
            _dbcontext.Venues.Add(obj);
            _dbcontext.SaveChanges();
            return obj.Id;
        }

        public int Delete(int id)
        {
            var del = _dbcontext.Venues.Find(id);
            _dbcontext.Venues.Remove(del);
            return _dbcontext.SaveChanges();
        }

        public List<Venue> FilterByName(Venue input)
        {
            return _dbcontext.Venues.Where(s => s.Description == input.Description).ToList();
        }

        public Venue GetById(int id)
        {
            return _dbcontext.Venues.Find(id);
        }

        public IQueryable<Venue> GetAll()
        {
            return _dbcontext.Venues;
        }

        public int Update(Venue obj)
        {
            _dbcontext.Entry(obj).State= EntityState.Modified;
            _dbcontext.SaveChanges();
            return obj.Id;
        }
    }
}
