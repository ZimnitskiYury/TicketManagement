namespace TicketManagement.PurchaseAPI.Models
{
    public class PaymentModel
    {
        /// <summary>
        /// Purchase Id.
        /// </summary>
        public int Id { get; set; }

        public int? TransactionId { get; set; }

        public string UserId { get; set; }

        public int EventSeatId { get; set; }

        public int Row { get; set; }

        public int Number { get; set; }

        public string EventAreaDescription { get; set; }

        public decimal Price { get; set; }

        public int EventId { get; set; }

        public string EventName { get; set; }
    }
}
