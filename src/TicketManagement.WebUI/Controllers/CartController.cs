using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.WebUI.Models.Cart;
using TicketManagement.WebUI.Services;

namespace TicketManagement.WebUI.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly OrderService _orderService;
        private readonly EventService _eventService;

        public CartController(OrderService orderService, EventService eventService)
        {
            _orderService = orderService;
            _eventService = eventService;
        }

        // GET: Cart
        [Authorize]
        public async Task<ActionResult> IndexAsync()
        {
            var model = new List<CartViewModel>();
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var orders = await _orderService.GetOrdersAsync(token);
            foreach (var order in orders)
            {
                var seat = await _eventService.GetSeatAsync(order.EventSeatId, token);
                var area = await _eventService.GetAreaAsync(seat.EventAreaId, token);
                var eve = await _eventService.GetEventAsync(area.EventId);
                model.Add(new CartViewModel
                {
                    EventData = eve,
                    EventSeat = seat,
                    AreaDescription = area.Description,
                    Id = order.Id,
                    Price = area.Price,
                });
            }

            return View(model);
        }
    }
}
