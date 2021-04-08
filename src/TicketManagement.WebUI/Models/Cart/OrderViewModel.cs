namespace TicketManagement.WebUI.Models.Cart
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int EventSeatId { get; set; }

        public int? TransactionId { get; set; }
    }
}
