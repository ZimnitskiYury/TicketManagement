﻿using System.Collections.Generic;

namespace TicketManagement.DesktopUI.Models
{
    public class AuthenticatedUserModel
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


        public string Token { get; set; }

        /// <summary>
        /// Roles of user.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}
