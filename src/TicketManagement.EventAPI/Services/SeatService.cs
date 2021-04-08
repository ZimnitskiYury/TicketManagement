using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

[assembly: InternalsVisibleTo("TicketManagement.UnitTests")]

namespace TicketManagement.EventAPI.Services
{
    public class SeatService
    {
        private readonly IRepository<Seat> _repository;

        public SeatService(IRepository<Seat> repository)
        {
            _repository = repository;
        }

        internal int DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        internal int Delete(Seat entity)
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

        internal IQueryable<Seat> GetAll()
        {
            return _repository.GetAll();
        }

        internal Seat GetById(int id)
        {
            return _repository.GetById(id);
        }

        internal int Insert(Seat entity)
        {
            // Checking for unique row and number in area
            if (_repository is ISeatRepositoryExtension extension)
            {
                if (!extension.FilterByRowAndNumberInArea(entity).Any())
                {
                    return _repository.Insert(entity);
                }
                else
                {
                    throw new ArgumentException("Row and number should be unique for area");
                }
            }
            else
            {
                throw new ArgumentException(nameof(_repository));
            }
        }

        internal int Update(Seat entity)
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
