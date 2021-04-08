using System;
using System.Linq;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

namespace TicketManagement.VenueAPI.Services
{
    public class LayoutService
    {
        private readonly IRepository<LayoutData> _repository;

        internal LayoutService(IRepository<LayoutData> repository)
        {
            _repository = repository;
        }

        internal int DeleteById(int id)
        {
            return _repository.Delete(id);
        }

        internal int Delete(LayoutData entity)
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

        internal IQueryable<LayoutData> GetAllValues()
        {
            return _repository.GetAll();
        }

        internal LayoutData GetById(int id)
        {
            return _repository.GetById(id);
        }

        internal int Insert(LayoutData entity)
        {
            // Checking for unique description
            if (_repository is ILayoutRepositoryExtension extension)
            {
                if (!extension.FilterByNameInVenue(entity).Any())
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

        internal int Update(LayoutData entity)
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
