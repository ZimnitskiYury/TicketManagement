using System.ComponentModel.DataAnnotations;

namespace TicketManagement.PurchaseAPI.Models
{
    public class UserModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string SurName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Language { get; set; }

        public decimal Balance { get; set; }
    }
}
