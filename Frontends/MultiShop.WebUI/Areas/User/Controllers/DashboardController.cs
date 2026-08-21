using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.UserServices;

namespace MultiShop.WebUI.Areas.User.Controllers;

[Area("User")]
[Authorize]
public sealed class DashboardController : Controller
{
    private readonly IUserService _userService;

    public DashboardController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserInfoAsync(cancellationToken);
        return View(user);
    }
}
