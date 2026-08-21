using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Basket;
using MultiShop.WebUI.Services.BasketServices;

namespace MultiShop.WebUI.ViewComponents;

public sealed class OrderSummaryViewComponent : ViewComponent
{
    private readonly IBasketService _basketService;

    public OrderSummaryViewComponent(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var basket = await _basketService.GetAsync(HttpContext.RequestAborted);
        return View(new CartViewModel { Basket = basket });
    }
}
