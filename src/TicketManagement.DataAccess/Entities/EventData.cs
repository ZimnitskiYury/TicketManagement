using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.DataAccess.Entities
{
    [Table("Event")]
    public class EventData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(120, ErrorMessage = "Название должно содержать не менее {2} символов.", MinimumLength = 4)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int LayoutId { get; set; }

        [Required]
        public Category Category { get; set; }

        [Column("DateBegin")]
        [Required]
        public DateTime StartDateTime { get; set; }

        [Column("DateEnd")]
        [Required]
        public DateTime EndDateTime { get; set; }
    }
}
