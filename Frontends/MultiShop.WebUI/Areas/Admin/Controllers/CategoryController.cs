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

    public async Task<IActionResult> UpdateCategory(string id, CancellationToken cancellationToken)
    {
        var model = await _categoryService.GetByIdAsync(id, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(updateCategoryDto);

        await _categoryService.UpdateAsync(updateCategoryDto, cancellationToken);
        return RedirectToAction("Index");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(string categoryId, CancellationToken cancellationToken)
    {
        await _categoryService.DeleteAsync(categoryId, cancellationToken);

        return RedirectToAction("Index");
    }

}
