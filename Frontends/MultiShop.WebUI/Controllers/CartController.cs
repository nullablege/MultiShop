using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers;

public class CartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
