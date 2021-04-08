using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;
using TicketManagement.EventAPI.Validation;

[assembly: InternalsVisibleTo("TicketManagement.UnitTests")]
[assembly: InternalsVisibleTo("TicketManagement.IntegrationTests")]

namespace TicketManagement.EventAPI.Services
{
    public class EventService
    {
        private readonly IRepository<EventData> _eventRepository;
        private readonly IRepository<LayoutData> _layoutRepository;
        private readonly IRepository<Seat> _seatRepository;

        public EventService(IRepository<EventData> eventRepository, IRepository<LayoutData> layoutRepository, IRepository<Seat> seatRepository)
        {
            _eventRepository = eventRepository;
            _layoutRepository = layoutRepository;
            _seatRepository = seatRepository;
        }

        internal int DeleteById(int id)
        {
            return _eventRepository.Delete(id);
        }

        internal int Delete(EventData entity)
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

        internal IQueryable<EventData> GetAll()
        {
            return _eventRepository.GetAll();
        }

        internal EventData GetById(int id)
        {
            return _eventRepository.GetById(id);
        }

        internal int Insert(EventData entity)
        {
            if (new EventValidator().Validate(entity))
            {
                if (ValidateForNoEventsOnVenueInSameTime(entity))
                {
                    if (ValidateAvailabilitySeats(entity))
                    {
                        var id = _eventRepository.Insert(entity);
                        return id;
                    }
                    else
                    {
                        throw new ArgumentException("Event must have seats");
                    }
                }
                else
                {
                    throw new ArgumentException("Event cannot occur for the same venue in the same time");
                }
            }
            else
            {
                throw new ArgumentException("Not Valid dates");
            }
        }

        private bool ValidateAvailabilitySeats(EventData entity)
        {
            if (_seatRepository is ISeatRepositoryExtension extension)
            {
                return extension.FilterSeatsByLayoutId(entity.LayoutId).Any();
            }
            else
            {
                throw new ArgumentException(nameof(_seatRepository));
            }
        }

        private bool ValidateForNoEventsOnVenueInSameTime(EventData entity)
        {
            // Take VenueId from layouts db
            var venueId = _layoutRepository.GetById(entity.LayoutId).VenueId;

            if (_eventRepository is IEventDataRepositoryExtension extension)
            {
                var events = extension.FilterByVenueId(venueId);

                // Generates a list of events for the same venue in the same time with input event
                var eventInSameTime = new List<EventData>();
                foreach (var eventData in events)
                {
                    if ((entity.StartDateTime <= eventData.StartDateTime) && (entity.EndDateTime > eventData.StartDateTime))
                    {
                        eventInSameTime.Add(eventData);
                    }

                    if ((entity.StartDateTime >= eventData.StartDateTime) && (entity.StartDateTime <= eventData.EndDateTime))
                    {
                        eventInSameTime.Add(eventData);
                    }
                }

                return !eventInSameTime.Any();
            }
            else
            {
                throw new ArgumentException(nameof(_eventRepository));
            }
        }

        internal int Update(EventData entity)
        {
            if (entity != null)
            {
                return _eventRepository.Update(entity);
            }
            else
            {
                throw new ArgumentNullException(nameof(entity));
            }
        }
    }
}
