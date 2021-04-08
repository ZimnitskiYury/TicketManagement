using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebUI.Models.Profile
{
    public class ProfileViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        [StringLength(50, ErrorMessage = "NameLength", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Login")]
        [StringLength(12, ErrorMessage = "LoginLength", MinimumLength = 5)]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [StringLength(100, ErrorMessage = "SurnameLength", MinimumLength = 3)]
        public string SurName { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Language")]
        [StringLength(5, MinimumLength = 2)]
        public string Language { get; set; }

        [Display(Name = "Balance")]
        public decimal Balance { get; set; }
    }
}
