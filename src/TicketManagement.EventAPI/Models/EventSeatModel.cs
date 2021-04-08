using TicketManagement.DataAccess.Entities;

namespace TicketManagement.EventAPI.Models
{
    public class EventSeatModel : EventSeat
    {
        public string EventAreaDescription { get; set; }

        public decimal Price { get; set; }
    }
}
