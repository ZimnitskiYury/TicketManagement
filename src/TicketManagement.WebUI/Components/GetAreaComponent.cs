using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebUI.Services;

namespace TicketManagement.WebUI.Components
{
    public class GetAreaComponent : ViewComponent
    {
        private readonly EventService _eventService;

        public GetAreaComponent(EventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var model = await _eventService.GetAreas(id, token);
            return View("_GetArea", model);
        }
    }
}
