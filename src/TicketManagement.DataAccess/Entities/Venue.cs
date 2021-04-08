using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.DataAccess.Entities
{
    [Table("Venue")]
    public class Venue
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Description { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }
    }
}
