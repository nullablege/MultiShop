using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Catalog.CategoryDTOs;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(createCategoryDto);

        await _categoryService.CreateAsync(createCategoryDto, cancellationToken);
        return RedirectToAction("Index");
    }

    public IActionResult UpdateCategory(string? id)
    {
        ViewData["CategoryId"] = id;
        return View();
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(string categoryId, CancellationToken cancellationToken)
    {
        await _categoryService.DeleteAsync(categoryId, cancellationToken);

        return RedirectToAction("Index");
    }

}
