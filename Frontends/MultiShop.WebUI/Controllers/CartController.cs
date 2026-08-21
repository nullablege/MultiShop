using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Basket;
using MultiShop.WebUI.Models.Discount;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly IBasketService _basketService;
    private readonly IProductService _productService;
    private readonly IDiscountService _discountService;

    public CartController(IBasketService basketService, IProductService productService, IDiscountService discountService)
    {
        _basketService = basketService;
        _productService = productService;
        _discountService = discountService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? couponCode, CancellationToken cancellationToken)
    {
        DiscountCouponDto? discountCode = null;
        var basket = await _basketService.GetAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(couponCode))
        {
            discountCode = await _discountService.GetByCodeAsync(couponCode, cancellationToken);
            if (discountCode == null)
            {
                TempData["CouponError"] = "Kupon Bulunamadi";
                return RedirectToAction(nameof(Index));
            }

            if (discountCode.ValidDate.Date < DateTime.UtcNow.Date || discountCode.IsActive == false)
            {
                TempData["CouponError"] = "Kupon kullanilamaz";
                return RedirectToAction(nameof(Index));
            }
        }
        return View(new CartViewModel { Basket = basket, CouponCode = discountCode?.Code, DiscountRate = discountCode?.Rate ?? 0 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(string productId, int quantity = 1, CancellationToken cancellationToken = default)
    {
        if (quantity < 1)
            quantity = 1;

        var product = await _productService.GetByIdAsync(productId, cancellationToken);
        if (product is null)
            return NotFound();

        await _basketService.AddItemAsync(new BasketItemDto
        {
            ProductId = product.ProductId,
            ProductName = product.ProductName,
            ProductImageUrl = product.CoverImageUrl,
            Price = product.ProductPrice,
            Quantity = quantity
        }, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(string productId, CancellationToken cancellationToken)
    {
        await _basketService.RemoveItemAsync(productId, cancellationToken);
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApplyCoupon(string code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            TempData["CouponError"] = "Kupon Kodu Girmelisiniz";
            return RedirectToAction(nameof(Index));
        }

        var discountCode = await _discountService.GetByCodeAsync(code, cancellationToken);

        if (discountCode == null)
        {
            TempData["CouponError"] = "Kupon Bulunamadi";
            return RedirectToAction(nameof(Index));
        }

        if (discountCode.ValidDate.Date < DateTime.UtcNow.Date || discountCode.IsActive == false)
        {
            TempData["CouponError"] = "Kupon kullanilamaz";
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index), new { couponCode = discountCode.Code });

    }
}
