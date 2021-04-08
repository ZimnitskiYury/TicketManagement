using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebUI.Services;

namespace TicketManagement.WebUI.Controllers
{
    public class VenueController : Controller
    {
        private readonly VenueService _venueService;

        public VenueController(VenueService venueService)
        {
            _venueService = venueService;
        }

        // GET: Venue
        public async Task<IActionResult> Index()
        {
            var model = await _venueService.GetAllAsync();
            return View(model);
        }

        // GET: Venue/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _venueService.GetVenueAsync(id);
            return View(model);
        }
    }
}
