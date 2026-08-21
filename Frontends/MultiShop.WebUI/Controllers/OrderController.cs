using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Order;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.OrderServices;

namespace MultiShop.WebUI.Controllers;

[Authorize]
public sealed class OrderController : Controller
{
    private readonly IBasketService _basketService;
    private readonly IOrderAddressService _orderAddressService;

    public OrderController(IBasketService basketService, IOrderAddressService orderAddressService)
    {
        _basketService = basketService;
        _orderAddressService = orderAddressService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAddress(CreateOrderAddressDto createOrderAddressDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(nameof(Index), createOrderAddressDto);

        await _orderAddressService.CreateAsync(createOrderAddressDto, cancellationToken);
        return RedirectToAction("Index", "Payment");
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var basket = await _basketService.GetAsync(cancellationToken);
        if (basket.BasketItems.Count == 0)
        {
            return RedirectToAction("Index", "Cart");
        }

        return View(new CreateOrderAddressDto());
    }
}
