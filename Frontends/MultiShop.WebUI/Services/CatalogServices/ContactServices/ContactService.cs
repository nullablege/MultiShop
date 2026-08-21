using MultiShop.WebUI.Models.Catalog.ContactDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.ContactServices;

public sealed class ContactService : IContactService
{
    private readonly HttpClient _httpClient;

    public ContactService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateAsync(CreateContactDto createContactDto, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("contacts", createContactDto, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
