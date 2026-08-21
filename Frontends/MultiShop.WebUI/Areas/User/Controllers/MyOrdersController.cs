using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.OrderServices;
using System.Threading.Tasks;

namespace MultiShop.WebUI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class MyOrdersController : Controller
    {
        private readonly IOrderHistoryService _orderHistoryService;
        public MyOrdersController(IOrderHistoryService orderHistoryService)
        {
            _orderHistoryService = orderHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var orderList = await _orderHistoryService.GetCurrentUserOrdersAsync(cancellationToken);
            return View(orderList);
        }
    }
}
