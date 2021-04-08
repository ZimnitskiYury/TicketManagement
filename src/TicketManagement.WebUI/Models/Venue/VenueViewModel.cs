using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebUI.Models.Venue
{
    public class VenueViewModel
    {
        [Key]
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
