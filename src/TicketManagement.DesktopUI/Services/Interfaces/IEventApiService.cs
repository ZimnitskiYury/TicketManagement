using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.DesktopUI.Models;

namespace TicketManagement.DesktopUI.Services.Interfaces
{
    public interface IEventApiService
    {
        Task<bool> AddEventAsync(EventModel eventModel);
        Task<bool> DeleteEventAsync(int id);
        Task<IEnumerable<EventModel>> GetAllAsync();
        Task<EventModel> GetEventAsync(int id);
        Task<bool> UpdateEventAsync(EventModel model);
    }
}