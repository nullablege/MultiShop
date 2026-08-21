using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Identity;
using MultiShop.WebUI.Services.CargoServices;
using MultiShop.WebUI.Services.UserServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public sealed class UserController : Controller
{
    private readonly IAdminUserService _adminUserService;
    private readonly ICargoCustomerService _cargoCustomerService;

    public UserController(
        IAdminUserService adminUserService,
        ICargoCustomerService cargoCustomerService)
    {
        _adminUserService = adminUserService;
        _cargoCustomerService = cargoCustomerService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var users = await _adminUserService.GetAllAsync(cancellationToken);
        return View(users);
    }

    public async Task<IActionResult> CargoProfile(
        string userId,
        CancellationToken cancellationToken)
    {
        var users = await _adminUserService.GetAllAsync(cancellationToken);
        var user = users.FirstOrDefault(item => item.Id == userId);

        if (user is null)
        {
            return NotFound();
        }

        var cargoCustomer = await _cargoCustomerService.GetByUserIdAsync(
            userId,
            cancellationToken);

        return View(new AdminUserCargoProfileViewModel
        {
            User = user,
            CargoCustomer = cargoCustomer
        });
    }
}
