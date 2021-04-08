using System.ComponentModel.DataAnnotations;

namespace TicketManagement.PurchaseAPI.Models
{
    /// <summary>
    /// Model of EventSeat from EventApi.
    /// </summary>
    public class EventSeatModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EventAreaId { get; set; }

        [Required]
        public int Row { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public StateSeat State { get; set; }

        public string EventAreaDescription { get; set; }

        public decimal Price { get; set; }
    }
}
