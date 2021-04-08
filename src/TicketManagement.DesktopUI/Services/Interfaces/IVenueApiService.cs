using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.DesktopUI.Models;

namespace TicketManagement.DesktopUI.Services.Interfaces
{
    public interface IVenueApiService
    {
        Task<bool> AddVenueAsync(VenueModel venueModel);
        Task<bool> DeleteVenueAsync(int id);
        Task<IEnumerable<VenueModel>> GetAllAsync();
        Task<bool> UpdateVenueAsync(VenueModel model);
    }
}