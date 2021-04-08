using System.Collections.Generic;
using TicketManagement.DataAccess.Entities;

namespace TicketManagement.DataAccess.Interfaces.RepositoryExtension
{
    public interface IAreaRepositoryExtension
    {
        List<Area> FilterByNameInLayout(Area input);
    }
}
