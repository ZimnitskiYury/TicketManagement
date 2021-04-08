using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TicketManagement.WebUI.Services;

namespace TicketManagement.WebUI.Components
{
    public class GetDiscountComponent : ViewComponent
    {
        private readonly IConfiguration _configuration;
        private readonly OrderService _orderService;

        public GetDiscountComponent(IConfiguration configuration, OrderService orderService)
        {
            _configuration = configuration;
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var token = HttpContext.Request.Cookies["secret_jwt_key"];
            var orders = await _orderService.GetOrdersAsync(token);
            int discount = _configuration.GetValue<int>("discount");
            int discountValue = _configuration.GetValue<int>("discountValue");
            ViewBag.Discount = orders.Count() >= discount ? "You get " + discountValue + "% discount" : "You need to make " + (discount - orders.Count()) + " more purchases";
            return View("_GetDiscountComponent");
        }
    }
}
