using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.CommentDTOs;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.CommentServices;

namespace MultiShop.WebUI.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IPublicCommentService _publicCommentService;


    public ProductsController(IProductService productService, IPublicCommentService publicCommentService)
    {
        _publicCommentService = publicCommentService;
        _productService = productService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> CreateComment(CreateCommentDto createCommentDto, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(createCommentDto.ProductId))
            return NotFound();

        if (!ModelState.IsValid)
        {
            var product = await _productService.GetByIdAsync(createCommentDto.ProductId, cancellationToken);

            if (product == null)
                return NotFound();

            return View("Details", product);
        }

        await _publicCommentService.CreateCommentAsync(createCommentDto, cancellationToken);
        return RedirectToAction(nameof(Details), new { productId = createCommentDto.ProductId });

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
