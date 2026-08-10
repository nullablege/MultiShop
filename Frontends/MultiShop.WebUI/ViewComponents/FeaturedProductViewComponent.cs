using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.ViewComponents;

public sealed class FeaturedProductViewComponent : ViewComponent
{
    private readonly IProductService _productService;

    public FeaturedProductViewComponent(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var products = await _productService.GetAllAsync(HttpContext.RequestAborted);
        return View(products.Take(8).ToArray());
    }
}
