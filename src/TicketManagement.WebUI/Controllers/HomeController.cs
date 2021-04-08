using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebUI.Models.Event;
using TicketManagement.WebUI.Services;

namespace TicketManagement.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventService _eventService;

        public HomeController(EventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<IActionResult> Index()
        {
            List<EventDataViewModel> events = await _eventService.GetAllAsync();
            return View(events.Where(x => x.StartDateTime >= DateTime.Now)
                              .OrderBy(x => x.StartDateTime)
                              .Take(4)
                              .ToList());
        }

        public ActionResult Contacts()
        {
            return View();
        }

        public ActionResult Faq()
        {
            return View();
        }

        public ActionResult Returns()
        {
            return View();
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            if (returnUrl == "%2F")
            {
                return View("Index");
            }

            return LocalRedirect(returnUrl);
        }
    }
}