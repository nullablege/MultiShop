using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CreateCategory()
    {
        return View();
    }

    public IActionResult UpdateCategory(string? id)
    {
        ViewData["CategoryId"] = id;
        return View();
    }
}
