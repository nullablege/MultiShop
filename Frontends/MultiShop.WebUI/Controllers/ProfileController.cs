using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.UserServices;

namespace MultiShop.WebUI.Controllers;

[Authorize]
public sealed class ProfileController : Controller
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await _userService.GetUserInfoAsync(cancellationToken);
        return View(model);
    }
}
