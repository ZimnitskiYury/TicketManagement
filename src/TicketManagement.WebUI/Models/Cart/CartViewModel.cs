using TicketManagement.WebUI.Models.Event;

namespace TicketManagement.WebUI.Models.Cart
{
    public class CartViewModel
    {
        public int Id { get; set; }

        public EventDataViewModel EventData { get; set; }

        public EventSeatViewModel EventSeat { get; set; }

        public string AreaDescription { get; set; }

        public decimal Price { get; set; }
    }
}
