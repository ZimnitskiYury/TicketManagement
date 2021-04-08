using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebUI.Models.Event
{
    public class EventSeatViewModel
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

        [Required]
        public string EventAreaDescription { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
