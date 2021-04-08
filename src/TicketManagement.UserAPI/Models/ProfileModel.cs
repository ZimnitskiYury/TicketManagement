using System.Collections.Generic;

namespace TicketManagement.UserAPI.Models
{
    public class ProfileModel
    {
        /// <summary>
        /// Unique guid.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// E-mail.
        /// </summary>
        public string Email { get; set; }

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

        /// <summary>
        /// Roles of user.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}
