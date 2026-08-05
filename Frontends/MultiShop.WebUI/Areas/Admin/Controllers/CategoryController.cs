using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellation)
    {
        var model = await _categoryService.GetAllAsync(cancellation);
        return View(model);
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
