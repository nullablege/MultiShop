using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Order;

namespace MultiShop.WebUI.ViewComponents;

public sealed class OrderAddressViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(CreateOrderAddressDto? model)
    {
        return View(model ?? new CreateOrderAddressDto());
    }
}
