namespace TicketManagement.UserAPI.Models
{
    /// <summary>
    /// Login model from UI.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Password for authorization.
        /// </summary>
        public string Password { get; set; }
    }
}
