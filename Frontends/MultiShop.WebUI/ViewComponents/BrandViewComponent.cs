using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.BrandServices;

namespace MultiShop.WebUI.ViewComponents;

public sealed class BrandViewComponent : ViewComponent
{
    private readonly IBrandService _brandService;

    public BrandViewComponent(IBrandService brandService)
    {
        _brandService = brandService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _brandService.GetAllAsync(HttpContext.RequestAborted));
    }
}
