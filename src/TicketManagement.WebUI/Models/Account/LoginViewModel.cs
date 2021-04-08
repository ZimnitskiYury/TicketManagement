using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebUI.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        [StringLength(12, ErrorMessage = "LoginLength", MinimumLength = 5)]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        [DataType(DataType.Password, ErrorMessage = "PasswordContains")]
        public string Password { get; set; }
    }
}
