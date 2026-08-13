using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(string categoryId, CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrWhiteSpace(categoryId))
            return View(await _productService.GetAllAsync(cancellationToken));


        return View(await _productService.GetByCategoryIdAsync(categoryId, cancellationToken));
    }

    public async Task<IActionResult> Details(string productId, CancellationToken cancellationToken = default)
    {
        var productDetail = await _productService.GetByIdAsync(productId, cancellationToken);
        if (productDetail == null)
            return NotFound();

        return View(productDetail);
    }
}
