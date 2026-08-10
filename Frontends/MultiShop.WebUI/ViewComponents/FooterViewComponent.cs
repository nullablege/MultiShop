using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.AboutServices;

namespace MultiShop.WebUI.ViewComponents;

public sealed class FooterViewComponent : ViewComponent
{
    private readonly IAboutService _aboutService;
    public FooterViewComponent(IAboutService aboutService) { _aboutService = aboutService; }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _aboutService.GetAllAsync(HttpContext.RequestAborted));
    }
}
