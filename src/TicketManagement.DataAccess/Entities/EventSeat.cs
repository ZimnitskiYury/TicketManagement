using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.DataAccess.Entities
{
    [Table("EventSeat")]
    public class EventSeat
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
    }
}
