namespace TicketManagement.UserAPI.Models
{
    /// <summary>
    /// Model from UI.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// E-mail.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// FirstName of user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Surname of user.
        /// </summary>
        public string SurName { get; set; }

        /// <summary>
        /// Preffered Language.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Amount of money.
        /// </summary>
        public decimal Balance { get; set; }
    }
}
