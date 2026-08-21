using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers;

[Authorize]
public sealed class PaymentController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}
