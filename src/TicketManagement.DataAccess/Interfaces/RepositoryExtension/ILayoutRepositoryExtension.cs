using System.Collections.Generic;
using TicketManagement.DataAccess.Entities;

namespace TicketManagement.DataAccess.Interfaces.RepositoryExtension
{
    public interface ILayoutRepositoryExtension
    {
        List<LayoutData> FilterByNameInVenue(LayoutData input);
    }
}
