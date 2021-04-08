using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebUI.Models.Event
{
    public class EventAreaViewModel
    {
            [Key]
            public int Id { get; set; }

            [Required]
            public int EventId { get; set; }

            [Required]
            [StringLength(200)]
            public string Description { get; set; }

            [Required]
            public int CoordX { get; set; }

            [Required]
            public int CoordY { get; set; }

            [Required]
            public decimal Price { get; set; }
    }
}
