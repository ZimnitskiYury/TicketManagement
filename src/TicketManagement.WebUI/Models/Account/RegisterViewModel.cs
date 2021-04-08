using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebUI.Models.Account
{
    public class RegisterViewModel
    {
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
        [EmailAddress(ErrorMessage ="InvalidEmail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!_%*?&])[A-Za-z\d$@$!%_*?&]{6,}", ErrorMessage = "PasswordContains")]
        [DataType(DataType.Password, ErrorMessage = "PasswordContains")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessage = "PasswordCompare")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Language")]
        public string Language { get; set; }
    }
}
