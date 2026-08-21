using MultiShop.WebUI.Models.Catalog.ContactDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.ContactServices;

public interface IContactService
{
    Task CreateAsync(CreateContactDto createContactDto, CancellationToken cancellationToken = default);
}
