using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TicketManagement.WebUI.Models.Cart;
using TicketManagement.WebUI.Services;

namespace TicketManagement.WebUI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        private readonly EventService _eventService;
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public OrderController(OrderService orderService, EventService eventService, UserService userService, IConfiguration configuration)
        {
            _orderService = orderService;
            _eventService = eventService;
            _userService = userService;
            _configuration = configuration;
        }

        // GET: Order
        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var model = await _eventService.GetSeatsAsync(id, token);
            return View(model);
        }

        /// <summary>
        /// Post for buying tickets.
        /// </summary>
        /// <param name="checkedseats">List of seat's id from ui.</param>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Buy(List<int> checkedseats)
        {
            foreach (var id in checkedseats)
            {
                var token = HttpContext.Request.Cookies["secret_jwt_key"];
                await _orderService.ReserveSeatAsync(id, token);
            }

            return RedirectToAction("Index", "Cart");
        }

        // GET: Payment
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> PaymentAsync(int id)
        {
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var payment = await _orderService.GetPaymentAsync(id, token);
            int discount = _configuration.GetValue<int>("discount");
            int discountValue = _configuration.GetValue<int>("discountValue");
            var orders = await _orderService.GetOrdersAsync(token);
            if (orders.Count() >= discount)
            {
                payment.Price -= payment.Price / 100 * discountValue;
            }

            return View(payment);
        }

        // GET: Payment
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> PaymentAsync([FromForm] CartViewModel model)
        {
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var user = await _userService.GetProfile(User.Identity.Name, token);
            if (user.Balance < model.Price)
            {
                return RedirectToAction("AddCash", "Order");
            }

            await _orderService.PayAsync(model.Id, token);
            return RedirectToAction("Index", "Cart");
        }

        // GET: Add Balance
        [Authorize]
        public ActionResult AddCash()
        {
            return View();
        }
    }
}