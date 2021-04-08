using System;
using System.Linq;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.BusinessLogic.Services
{
    public class PurchaseService
    {
        private readonly IRepository<Purchase> _repository;

        public PurchaseService(IRepository<Purchase> repository)
        {
            _repository = repository;
        }

        internal int DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        internal int Delete(Purchase entity)
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

        internal IQueryable<Purchase> GetAll()
        {
            return _repository.GetAll();
        }

        internal Purchase GetById(int id)
        {
            return _repository.GetById(id);
        }

        internal int Insert(Purchase entity)
        {
            return _repository.Insert(entity);
        }

        internal int Update(Purchase entity)
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
