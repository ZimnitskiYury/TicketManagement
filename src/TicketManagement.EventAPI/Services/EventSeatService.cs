using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;

[assembly: InternalsVisibleTo("TicketManagement.UnitTests")]

namespace TicketManagement.EventAPI.Services
{
    public class EventSeatService
    {
        private readonly IRepository<EventSeat> _repository;

        public EventSeatService(IRepository<EventSeat> repository)
        {
            _repository = repository;
        }

        internal int Delete(EventSeat entity)
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

        internal IQueryable<EventSeat> GetAll()
        {
            return _repository.GetAll();
        }

        internal EventSeat GetById(int id)
        {
            return _repository.GetById(id);
        }

        internal int Insert(EventSeat entity)
        {
            return _repository.Insert(entity);
        }

        internal int Update(EventSeat entity)
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
