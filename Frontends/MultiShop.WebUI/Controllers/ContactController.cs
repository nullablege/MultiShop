using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.Catalog.ContactDTOs;
using MultiShop.WebUI.Services.CatalogServices.ContactServices;

namespace MultiShop.WebUI.Controllers;

public class ContactController : Controller
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CreateContactDto createContactDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(createContactDto);
        }

        await _contactService.CreateAsync(createContactDto, cancellationToken);
        TempData["ContactSuccess"] = "Mesajınız başarıyla gönderildi.";
        return RedirectToAction(nameof(Index));
    }
}
