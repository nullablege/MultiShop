using MultiShop.WebUI.Models.Catalog.SpecialOfferDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.SpecialOfferServices
{
    public interface ISpecialOfferService
    {
        Task<IReadOnlyList<ResultSpecialOfferDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UpdateSpecialOfferDto?> GetByIdAsync(string specialOfferId, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateSpecialOfferDto createSpecialOfferDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateSpecialOfferDto updateSpecialOfferDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(string specialOfferId, CancellationToken cancellationToken = default);
    }
}
