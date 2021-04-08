using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;

[assembly: InternalsVisibleTo("TicketManagement.UnitTests")]

namespace TicketManagement.EventAPI.Services
{
    public class EventAreaService
    {
        private readonly IRepository<EventArea> _repository;

        public EventAreaService(IRepository<EventArea> repository)
        {
            _repository = repository;
        }

        internal int Delete(EventArea entity)
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

        internal int DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        internal IQueryable<EventArea> GetAll()
        {
            return _repository.GetAll();
        }

        internal EventArea GetById(int id)
        {
            return _repository.GetById(id);
        }

        internal int Insert(EventArea entity)
        {
            return _repository.Insert(entity);
        }

        internal int Update(EventArea entity)
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
