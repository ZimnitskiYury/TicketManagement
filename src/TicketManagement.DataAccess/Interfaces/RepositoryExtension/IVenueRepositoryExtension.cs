using System.Collections.Generic;
using TicketManagement.DataAccess.Entities;

namespace TicketManagement.DataAccess.Interfaces.RepositoryExtension
{
    public interface IVenueRepositoryExtension
    {
        List<Venue> FilterByName(Venue input);
    }
}
