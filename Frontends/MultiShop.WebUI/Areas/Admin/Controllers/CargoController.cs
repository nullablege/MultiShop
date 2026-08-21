using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.CargoDTOs;
using MultiShop.WebUI.Services.CargoServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public sealed class CargoController : Controller
{
    private readonly ICargoCompanyService _cargoCompanyService;

    public CargoController(ICargoCompanyService cargoCompanyService)
    {
        _cargoCompanyService = cargoCompanyService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var cargoCompanies = await _cargoCompanyService.GetAllAsync(cancellationToken);
        return View(cargoCompanies);
    }

    [HttpGet]
    public IActionResult CreateCargoCompany()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCargoCompany(
        CreateCargoCompanyDto createCargoCompanyDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(createCargoCompanyDto);
        }

        await _cargoCompanyService.CreateAsync(createCargoCompanyDto, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> UpdateCargoCompany(
        int id,
        CancellationToken cancellationToken)
    {
        var cargoCompany = await _cargoCompanyService.GetByIdAsync(id, cancellationToken);

        if (cargoCompany is null)
        {
            return NotFound();
        }

        return View(cargoCompany);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCargoCompany(
        UpdateCargoCompanyDto updateCargoCompanyDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(updateCargoCompanyDto);
        }

        await _cargoCompanyService.UpdateAsync(updateCargoCompanyDto, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCargoCompany(
        int cargoCompanyId,
        CancellationToken cancellationToken)
    {
        await _cargoCompanyService.DeleteAsync(cargoCompanyId, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
