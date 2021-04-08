using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebUI.Models.Event;
using TicketManagement.WebUI.Services;

namespace TicketManagement.WebUI.Controllers
{
    public class EventController : Controller
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<EventDataViewModel> model = await _eventService.GetAllAsync();
            return View(model.Where(x => x.StartDateTime >= DateTime.Now).OrderBy(x => x.StartDateTime).ToList());
        }

        // GET: Event/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _eventService.GetEventAsync(id);
            return View(model);
        }

        // GET: Event/Category/Concert
        [Authorize]
        public async Task<IActionResult> Category(string id)
        {
            var input = (Category)Enum.Parse(typeof(Category), id);
            var model = await _eventService.GetAllAsync();
            ViewBag.Category = input;
            return View(model.Where(x => x.Category == input));
        }

        // GET: Event/Create
        [Authorize(Roles = "Event, Venue, Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [Authorize(Roles = "Event, Venue, Administrator")]
        public async Task<IActionResult> Create([FromForm] EventDataViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var response = await _eventService.CreateEventAsync(model, token);
            var idevent = int.Parse(response);
            return RedirectToAction("Price", "Event", new { id = idevent });
        }

        // GET: Event/Price
        [Authorize(Roles = "Event, Venue, Administrator")]
        public async Task<IActionResult> Price(int id)
        {
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var model = await _eventService.GetAreas(id, token);
            return View(model);
        }

        // POST: Event/Price
        [HttpPost]
        [Authorize(Roles = "Event, Venue, Administrator")]
        public async Task<IActionResult> Price(IEnumerable<EventAreaViewModel> model)
        {
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            int r = 0;
            foreach (var area in model)
                {
                await _eventService.UpdatePriceAsync(area, token);
                r = area.EventId;
                }

            return RedirectToAction("Details", new { id = r });
        }

        // GET: Event/Edit/5
        [HttpGet]
        [Authorize(Roles = "Event, Venue, Administrator")]
#pragma warning disable S4144 // Methods should not have identical implementations
        public async Task<IActionResult> Edit(int id)
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            var model = await _eventService.GetEventAsync(id);
            return View(model);
        }

        // POST: Event/Edit/5
        [HttpPost]
        [Authorize(Roles = "Event, Venue, Administrator")]
        public async Task<IActionResult> Edit([FromForm] EventDataViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var response = await _eventService.UpdateEventAsync(model, token);
            return RedirectToAction("Details", "Event", new { id = response });
        }

        // GET: Event/Delete/5
        [Authorize(Roles = "Event, Venue, Administrator")]
#pragma warning disable S4144 // Methods should not have identical implementations
        public async Task<IActionResult> Delete(int id)
#pragma warning restore S4144 // Methods should not have identical implementations
        {
            var model = await _eventService.GetEventAsync(id);
            return View(model);
        }

        // POST: Event/Delete/5
        [Authorize(Roles = "Event, Venue, Administrator")]
        [HttpPost]
        public async Task<IActionResult> Delete(EventDataViewModel model)
        {
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            await _eventService.DeleteEventAsync(model.Id, token);
            return RedirectToAction("Index", "Event");
        }
    }
}
