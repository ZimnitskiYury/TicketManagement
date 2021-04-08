using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

namespace TicketManagement.DataAccess.Repository.EF
{
    public class AreaRepository : IRepository<Area>, IAreaRepositoryExtension
    {
        private readonly DataContext _dbcontext;

        public AreaRepository(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public int Insert(Area obj)
        {
            _dbcontext.Areas.Add(obj);
            _dbcontext.SaveChanges();
            return obj.Id;
        }

        public int Delete(int id)
        {
            var del = _dbcontext.Areas.Find(id);
            _dbcontext.Areas.Remove(del);
            return _dbcontext.SaveChanges();
        }

        public List<Area> FilterByNameInLayout(Area input)
        {
            return _dbcontext.Areas.Where(x => (x.Description == input.Description) && (x.LayoutId == input.LayoutId)).ToList();
        }

        public Area GetById(int id)
        {
            return _dbcontext.Areas.Find(id);
        }

        public IQueryable<Area> GetAll()
        {
            return _dbcontext.Areas;
        }

        public int Update(Area obj)
        {
            _dbcontext.Entry(obj).State= EntityState.Modified;
            _dbcontext.SaveChanges();
            return obj.Id;
        }
    }
}
