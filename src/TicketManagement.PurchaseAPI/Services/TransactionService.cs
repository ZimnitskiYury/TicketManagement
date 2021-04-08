using System;
using System.Linq;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.PurchaseAPI.Services
{
    /// <summary>
    /// Service for transact opertions with repository.
    /// </summary>
    public class TransactionService
    {
        private readonly IRepository<Transaction> _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class.
        /// </summary>
        /// <param name="repository">Transaction repository.</param>
        public TransactionService(IRepository<Transaction> repository)
        {
            _repository = repository;
        }

        internal int DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        internal int Delete(Transaction entity)
        {
            if (entity != null)
            {
                return DeleteById(entity.Id);
            }
            else
            {
                throw new ArgumentNullException(nameof(entity));
            }
        }

        internal IQueryable<Transaction> GetAll()
        {
            return _repository.GetAll();
        }

        internal Transaction GetById(int id)
        {
            return _repository.GetById(id);
        }

        internal int Insert(Transaction entity)
        {
            return _repository.Insert(entity);
        }

        internal int Update(Transaction entity)
        {
            if (entity != null)
            {
                return _repository.Update(entity);
            }
            else
            {
                throw new ArgumentNullException(nameof(entity));
            }
        }
    }
}
