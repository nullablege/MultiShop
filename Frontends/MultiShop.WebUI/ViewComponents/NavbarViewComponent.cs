using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;

namespace MultiShop.WebUI.ViewComponents;

public sealed class NavbarViewComponent : ViewComponent
{
    private readonly ICategoryService _categoryService;

    public NavbarViewComponent(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _categoryService.GetAllAsync(HttpContext.RequestAborted));
    }
}
