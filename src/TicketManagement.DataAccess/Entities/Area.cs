using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.DataAccess.Entities
{
    [Table("Area")]
    public class Area
    {
        public int Id { get; set; }

        public int LayoutId { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        public int CoordX { get; set; }

        public int CoordY { get; set; }
    }
}