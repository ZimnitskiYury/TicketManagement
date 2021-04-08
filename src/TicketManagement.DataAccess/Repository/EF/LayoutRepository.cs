using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

namespace TicketManagement.DataAccess.Repository.EF
{
    public class LayoutRepository : IRepository<LayoutData>, ILayoutRepositoryExtension
    {
        private readonly DataContext _dbcontext;

        public LayoutRepository(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public int Insert(LayoutData obj)
        {
            _dbcontext.Layouts.Add(obj);
            _dbcontext.SaveChanges();
            return obj.Id;
        }

        public int Delete(int id)
        {
            var del = _dbcontext.Layouts.Find(id);
            _dbcontext.Layouts.Remove(del);
            return _dbcontext.SaveChanges();
        }

        public List<LayoutData> FilterByNameInVenue(LayoutData input)
        {
            return _dbcontext.Layouts.Where(x => (x.Description == input.Description) && (x.VenueId == input.VenueId)).ToList();
        }

        public LayoutData GetById(int id)
        {
            return _dbcontext.Layouts.Find(id);
        }

        public IQueryable<LayoutData> GetAll()
        {
            return _dbcontext.Layouts;
        }

        public int Update(LayoutData obj)
        {
            _dbcontext.Entry(obj).State= EntityState.Modified;
            _dbcontext.SaveChanges();
            return obj.Id;
        }
    }
}
