using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

[assembly: InternalsVisibleTo("TicketManagement.UnitTests")]

namespace TicketManagement.EventAPI.Services
{
    public class AreaService
    {
        private readonly IRepository<Area> _repository;

        public AreaService(IRepository<Area> repository)
        {
            _repository = repository;
        }

        internal int DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        internal int Delete(Area entity)
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

        internal IQueryable<Area> GetAll()
        {
            return _repository.GetAll();
        }

        internal Area GetById(int id)
        {
            return _repository.GetById(id);
        }

        internal int Insert(Area entity)
        {
            // Checking for unique description into base
            if (_repository is IAreaRepositoryExtension extension)
            {
                if (!extension.FilterByNameInLayout(entity).Any())
                {
                    return _repository.Insert(entity);
                }
                else
                {
                    throw new ArgumentException("Description not unique");
                }
            }
            else
            {
                throw new ArgumentException(nameof(_repository));
            }
        }

        internal int Update(Area entity)
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
