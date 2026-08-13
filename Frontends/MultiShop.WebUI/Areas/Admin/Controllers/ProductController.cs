using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Catalog.ProductDTOs;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await _productService.GetAllAsync(cancellationToken);
        return View(model);
    }

    public async Task<IActionResult> CreateProduct(CancellationToken cancellationToken)
    {
        await LoadCategoriesAsync(cancellationToken);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(cancellationToken);
            return View(createProductDto);
        }

        await _productService.CreateAsync(createProductDto, cancellationToken);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> UpdateProduct(string id, CancellationToken cancellationToken)
    {
        await LoadCategoriesAsync(cancellationToken);
        var model = await _productService.GetForUpdateAsync(id, cancellationToken);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(cancellationToken);
            return View(updateProductDto);
        }

        await _productService.UpdateAsync(updateProductDto, cancellationToken);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(string productId, CancellationToken cancellationToken)
    {
        await _productService.DeleteAsync(productId, cancellationToken);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ProductListWithCategory(CancellationToken cancellationToken)
    {
        var model = await _productService.GetWithCategoryAsync(cancellationToken);
        return View(model);
    }

    private async Task LoadCategoriesAsync(CancellationToken cancellationToken)
    {
        ViewBag.Categories = await _categoryService.GetAllAsync(cancellationToken);
    }
}
