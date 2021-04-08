using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repository.EF
{
    public class TransactionRepository : IRepository<Transaction>
    {
        private readonly DataContext _dbcontext;

        public TransactionRepository(DataContext dataContext)
        {
            _dbcontext = dataContext;
        }

        public int Insert(Transaction obj)
        {
            _dbcontext.Transactions.Add(obj);
            _dbcontext.SaveChanges();
            return obj.Id;
        }

        public int Delete(int id)
        {
            var del = _dbcontext.Transactions.Find(id);
            _dbcontext.Transactions.Remove(del);
            return _dbcontext.SaveChanges();
        }

        public Transaction GetById(int id)
        {
            return _dbcontext.Transactions.Find(id);
        }

        public IQueryable<Transaction> GetAll()
        {
            return _dbcontext.Transactions;
        }

        public int Update(Transaction obj)
        {
            _dbcontext.Entry(obj).State= EntityState.Modified;
            _dbcontext.SaveChanges();
            return obj.Id;
        }
    }
}
