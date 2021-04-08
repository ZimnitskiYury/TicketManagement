using System.Collections.Generic;
using TicketManagement.DataAccess.Entities;

namespace TicketManagement.DataAccess.Interfaces.RepositoryExtension
{
    public interface ISeatRepositoryExtension
    {
        List<Seat> FilterByRowAndNumberInArea(Seat obj);

        List<Seat> FilterSeatsByLayoutId(int id);
    }
}
