using System;
using System.Linq;
using System.Runtime.CompilerServices;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;

[assembly: InternalsVisibleTo("TicketManagement.UnitTests")]

namespace TicketManagement.VenueAPI.Services
{
    public class VenueService
    {
        private readonly IRepository<Venue> _repository;
        private readonly IRepository<LayoutData> _layoutRepository;
        private readonly IRepository<Area> _areaRepository;
        private readonly IRepository<Seat> _seatRepository;

        public VenueService(IRepository<Venue> repository, IRepository<LayoutData> layoutRepository, IRepository<Seat> seatRepository, IRepository<Area> areaRepository)
        {
            _areaRepository = areaRepository;
            _seatRepository = seatRepository;
            _layoutRepository = layoutRepository;
            _repository = repository;
        }

        internal int Delete(Venue entity)
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
            var layoutByVenueId = _layoutRepository.GetAll().Where(x => x.VenueId == id).ToList();

            var areaByVenueId = (from layout in layoutByVenueId
                                 join area in _areaRepository.GetAll() on layout.Id equals area.LayoutId
                                 select area).ToList();

            var seatsByVenueId = (from area in areaByVenueId
                                  join seat in _seatRepository.GetAll() on area.Id equals seat.AreaId
                                  select seat).ToList();
            foreach (var seat in seatsByVenueId)
            {
                _seatRepository.Delete(seat.Id);
            }

            foreach (var area in areaByVenueId)
            {
                _areaRepository.Delete(area.Id);
            }

            foreach (var layout in layoutByVenueId)
            {
                _layoutRepository.Delete(layout.Id);
            }

            return _repository.Delete(id);
        }

        internal IQueryable<Venue> GetAll()
        {
            return _repository.GetAll();
        }

        internal Venue GetById(int id)
        {
            return _repository.GetById(id);
        }

        internal int Insert(Venue entity)
        {
            // Checking for unique description into base
            if (_repository is IVenueRepositoryExtension extension)
            {
                if (!extension.FilterByName(entity).Any())
                {
                    return _repository.Insert(entity);
                }
                else
                {
                    throw new ArgumentException("Not Unique name of venue");
                }
            }
            else
            {
                throw new ArgumentException(nameof(_repository));
            }
        }

        internal int Update(Venue entity)
        {
            return entity != null ? _repository.Update(entity) : throw new ArgumentNullException(nameof(entity));
        }
    }
}