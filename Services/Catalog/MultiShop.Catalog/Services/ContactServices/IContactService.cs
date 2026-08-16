using MultiShop.Catalog.DTOs.ContactDTOs;

namespace MultiShop.Catalog.Services.ContactServices
{
    public interface IContactService
    {
        Task CreateAsync(CreateContactDto createContactDto, CancellationToken cancellationToken = default);
    }
}
