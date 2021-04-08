using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repository.EF
{
    public class PurchaseRepository : IRepository<Purchase>
    {
        private readonly DataContext _dbcontext;

        public PurchaseRepository(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public int Insert(Purchase obj)
        {
            _dbcontext.Purchases.Add(obj);
            _dbcontext.SaveChanges();
            return obj.Id;
        }

        public int Delete(int id)
        {
            var del = _dbcontext.Purchases.Find(id);
            _dbcontext.Purchases.Remove(del);
            return _dbcontext.SaveChanges();
        }

        public Purchase GetById(int id)
        {
            return _dbcontext.Purchases.Find(id);
        }

        public IQueryable<Purchase> GetAll()
        {
            return _dbcontext.Purchases;
        }

        public int Update(Purchase obj)
        {
            _dbcontext.Entry(obj).State= EntityState.Modified;
            _dbcontext.SaveChanges();
            return obj.Id;
        }
    }
}
