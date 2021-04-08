using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TicketManagement.UserAPI.DataAccess
{
    /// <summary>
    /// Added more property to default identity user.
    /// </summary>
    public class User : IdentityUser
    {
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
        [Column(TypeName = "decimal(8,2)")]
        public decimal Balance { get; set; }
    }
}
