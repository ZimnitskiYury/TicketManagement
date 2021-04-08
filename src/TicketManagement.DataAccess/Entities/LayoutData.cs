using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.DataAccess.Entities
{
    [Table("Layout")]
    public class LayoutData
    {
        public int Id { get; set; }

        public int VenueId { get; set; }

        [Required]
        [StringLength(120)]
        public string Description { get; set; }
    }
}