using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TicketManagement.WebUI.Models.Account;
using TicketManagement.WebUI.Services;

namespace TicketManagement.WebUI.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public AccountController(UserService userService, IConfiguration config)
        {
            _configuration = config;
            _userService = userService;
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.Return = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userService.Login(model);
            if (result.IsSuccessStatusCode)
            {
                var token = await result.Content.ReadAsStringAsync();
                HttpContext.Response.Cookies.Append("secret_jwt_key", token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Неудачная попытка входа.");
                return View(model);
            }
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_configuration.GetValue<string>("userApi")),
            };

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("FirstName", model.FirstName),
                new KeyValuePair<string, string>("Password", model.Password),
                new KeyValuePair<string, string>("SurName", model.SurName),
                new KeyValuePair<string, string>("Email", model.Email),
                new KeyValuePair<string, string>("Login", model.Login),
                new KeyValuePair<string, string>("Language", model.Language),
            });

            var result = await httpClient.PostAsync("users/register", formContent);
            if (result.IsSuccessStatusCode)
            {
                var token = await result.Content.ReadAsStringAsync();
                HttpContext.Response.Cookies.Append("secret_jwt_key", token, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                });
                return RedirectToAction("Index", "Home");
            }

            return Forbid();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            HttpContext.Response.Cookies.Delete("secret_jwt_key");
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}