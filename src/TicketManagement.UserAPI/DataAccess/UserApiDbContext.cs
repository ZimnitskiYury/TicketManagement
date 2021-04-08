using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TicketManagement.UserAPI.DataAccess
{
    /// <summary>
    /// DbContext for working with Identity.
    /// </summary>
    public class UserApiDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserApiDbContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public UserApiDbContext(DbContextOptions<UserApiDbContext> options)
            : base(options)
        {
        }
    }
}
