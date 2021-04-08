using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketManagement.DataAccess.Entities
{
    [Table("Purchase")]
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("UserId")]
        public string UserId { get; set; }

        [Required]
        [Column("EventSeatId")]
        public int EventSeatId { get; set; }

        [Column("TransactionId")]
        public int? TransactionId { get; set; }
    }
}
