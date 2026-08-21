using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.UserServices;

namespace MultiShop.WebUI.Areas.User.ViewComponents;

public sealed class UserAreaNavbarViewComponent : ViewComponent
{
    private readonly IUserService _userService;

    public UserAreaNavbarViewComponent(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userService.GetUserInfoAsync(HttpContext.RequestAborted);
        return View(user);
    }
}
