using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Catalog.AboutDTOs;
using MultiShop.WebUI.Services.CatalogServices.AboutServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class AboutController : Controller
{
    private readonly IAboutService _aboutService;
    public AboutController(IAboutService aboutService) { _aboutService = aboutService; }
    public async Task<IActionResult> Index(CancellationToken cancellationToken) => View(await _aboutService.GetAllAsync(cancellationToken));
    public IActionResult CreateAbout() => View();
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAbout(CreateAboutDto dto, CancellationToken cancellationToken) { if (!ModelState.IsValid) return View(dto); await _aboutService.CreateAsync(dto, cancellationToken); return RedirectToAction("Index"); }
    public async Task<IActionResult> UpdateAbout(string id, CancellationToken cancellationToken) { var model = await _aboutService.GetByIdAsync(id, cancellationToken); return model == null ? NotFound() : View(model); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAbout(UpdateAboutDto dto, CancellationToken cancellationToken) { if (!ModelState.IsValid) return View(dto); await _aboutService.UpdateAsync(dto, cancellationToken); return RedirectToAction("Index"); }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAbout(string aboutId, CancellationToken cancellationToken) { await _aboutService.DeleteAsync(aboutId, cancellationToken); return RedirectToAction("Index"); }
}
