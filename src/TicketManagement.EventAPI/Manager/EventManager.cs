using System.Collections.Generic;
using System.Linq;
using TicketManagement.DataAccess.Entities;
using TicketManagement.EventAPI.Models;
using TicketManagement.EventAPI.Services;

namespace TicketManagement.EventAPI.Manager
{
    public class EventManager
    {
        private readonly EventService _eventService;
        private readonly EventAreaService _eventAreaService;
        private readonly EventSeatService _eventSeatService;
        private readonly SeatService _seatService;
        private readonly AreaService _areaService;

        public EventManager(EventService eventService, EventAreaService eventAreaService, EventSeatService eventSeatService, SeatService seatService, AreaService areaService)
        {
            _eventService = eventService;
            _eventAreaService = eventAreaService;
            _eventSeatService = eventSeatService;
            _seatService = seatService;
            _areaService = areaService;
        }

        internal int Insert(EventData eventData)
        {
            int id = _eventService.Insert(eventData);
            foreach (var area in _areaService.GetAll().Where(x => x.LayoutId == eventData.LayoutId).ToList())
            {
                var idarea = _eventAreaService.Insert(new EventArea
                {
                    Description = area.Description,
                    CoordX = area.CoordX,
                    CoordY = area.CoordY,
                    EventId = id,
                    Price = 1000,
                });
                foreach (var seat in _seatService.GetAll().Where(x => x.AreaId == area.Id).ToList())
                {
                    _eventSeatService.Insert(new EventSeat
                    {
                        EventAreaId = idarea,
                        Number = seat.Number,
                        Row = seat.Row,
                        State = StateSeat.Free,
                    });
                }
            }

            return id;
        }

        internal IQueryable<EventData> GetAll()
        {
            return _eventService.GetAll();
        }

        internal IEnumerable<EventArea> GetAreasOfEvent(int idevent)
        {
            var areas = _eventAreaService.GetAll().Where(x => x.EventId == idevent);
            return areas;
        }

        internal int UpdateArea(EventArea eventArea)
        {
            return _eventAreaService.Update(eventArea);
        }

        internal int UpdateSeat(EventSeat eventSeat)
        {
            return _eventSeatService.Update(eventSeat);
        }

        internal EventData GetById(int id)
        {
            return _eventService.GetById(id);
        }

        internal int Update(EventData entity)
        {
            return _eventService.Update(entity);
        }

        internal int Delete(int id)
        {
            var areasForDel = _eventAreaService.GetAll().Where(x => x.EventId == id).ToList();
            foreach (var area in areasForDel)
            {
                var seatsForDel = _eventSeatService.GetAll().Where(x => x.EventAreaId == area.Id).ToList();
                foreach (var seat in seatsForDel)
                {
                    _eventSeatService.DeleteById(seat.Id);
                }

                _eventAreaService.DeleteById(area.Id);
            }

            return _eventService.DeleteById(id);
        }

        internal IQueryable<EventSeat> GetSeats(int id)
        {
            var model = from area in _eventAreaService.GetAll()
                        join seat in _eventSeatService.GetAll() on area.Id equals seat.EventAreaId
                        where area.EventId == id
                        orderby seat.EventAreaId
                        select new EventSeatModel
                        {
                            Id = seat.Id,
                            Row = seat.Row,
                            Number = seat.Number,
                            State = seat.State,
                            EventAreaId = seat.EventAreaId,
                            EventAreaDescription = area.Description,
                            Price = area.Price,
                        };
            return model;
        }

        internal EventSeat GetSeat(int id)
        {
            var model = from seat in _eventSeatService.GetAll()
                        join area in _eventAreaService.GetAll() on seat.EventAreaId equals area.Id
                        where seat.Id == id
                        select new EventSeatModel
                        {
                            Id = seat.Id,
                            Row = seat.Row,
                            Number = seat.Number,
                            State = seat.State,
                            EventAreaId = seat.EventAreaId,
                            EventAreaDescription = area.Description,
                            Price = area.Price,
                        };
            return model.First();
        }

        internal EventArea GetArea(int id)
        {
            var model = _eventAreaService.GetById(id);
            return model;
        }
    }
}
