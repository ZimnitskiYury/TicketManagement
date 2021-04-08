using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebUI.Models.Profile;
using TicketManagement.WebUI.Services;

namespace TicketManagement.WebUI.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserService _userService;

        public ProfileController(UserService userService)
        {
            _userService = userService;
        }

        // GET: Profile
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var login = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var user = await _userService.GetProfile(login, token);
            return View(user);
        }

        // GET: Edit
        public async Task<IActionResult> EditAsync()
        {
            var login = HttpContext.User.FindFirst("sub")?.Value;
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var user = await _userService.GetProfile(login, token);
            return View(user);
        }

        // Post: Edit
        [HttpPost]
        public async Task<ActionResult> EditAsync([FromForm]ProfileViewModel model)
        {
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var response = await _userService.EditProfile(model, token);
            if (response == 1)
            {
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                return View(model);
            }
        }
    }
}