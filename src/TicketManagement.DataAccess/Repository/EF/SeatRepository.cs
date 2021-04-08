using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

namespace TicketManagement.DataAccess.Repository.EF
{
    public class SeatRepository : IRepository<Seat>, ISeatRepositoryExtension
    {
        private readonly DataContext _dbcontext;

        public SeatRepository(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public int Insert(Seat obj)
        {
            _dbcontext.Seats.Add(obj);
            _dbcontext.SaveChanges();
            return obj.Id;
        }

        public int Delete(int id)
        {
            var del = _dbcontext.Seats.Find(id);
            _dbcontext.Seats.Remove(del);
            return _dbcontext.SaveChanges();
        }

        public List<Seat> FilterByRowAndNumberInArea(Seat obj)
        {
            return _dbcontext.Seats.Where(x => (x.Row == obj.Row) && (x.Number == obj.Number) && (x.AreaId == obj.AreaId)).ToList();
        }

        public List<Seat> FilterSeatsByLayoutId(int id)
        {
            var seats = from seat in _dbcontext.Seats
                        join area in _dbcontext.Areas on seat.AreaId equals area.Id
                        where area.LayoutId == id
                        select seat;
            return seats.ToList();
        }

        public Seat GetById(int id)
        {
            return _dbcontext.Seats.Find(id);
        }

        public IQueryable<Seat> GetAll()
        {
            return _dbcontext.Seats;
        }

        public int Update(Seat obj)
        {
            _dbcontext.Entry(obj).State= EntityState.Modified;
            _dbcontext.SaveChanges();
            return obj.Id;
        }
    }
}
