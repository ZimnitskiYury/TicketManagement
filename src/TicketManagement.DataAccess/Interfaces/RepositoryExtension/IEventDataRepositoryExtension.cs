using System.Collections.Generic;
using TicketManagement.DataAccess.Entities;

namespace TicketManagement.DataAccess.Interfaces.RepositoryExtension
{
    public interface IEventDataRepositoryExtension
    {
        List<EventData> FilterByVenueId(int input);
    }
}
