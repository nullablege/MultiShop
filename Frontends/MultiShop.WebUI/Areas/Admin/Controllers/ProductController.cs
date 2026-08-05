using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CreateProduct()
    {
        return View();
    }

    public IActionResult UpdateProduct(string? id)
    {
        ViewData["ProductId"] = id;
        return View();
    }

    public IActionResult ProductListWithCategory()
    {
        return View();
    }
}
