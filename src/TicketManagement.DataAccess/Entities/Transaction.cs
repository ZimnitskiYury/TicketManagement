using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Entities
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime DateOfTransaction { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
