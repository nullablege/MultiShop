using MultiShop.WebUI.Models.Catalog.OfferDiscountDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.OfferDiscountServices
{
    public interface IOfferDiscountService
    {
        Task<IReadOnlyList<ResultOfferDiscountDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UpdateOfferDiscountDto?> GetByIdAsync(string offerDiscountId, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateOfferDiscountDto createOfferDiscountDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateOfferDiscountDto updateOfferDiscountDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(string offerDiscountId, CancellationToken cancellationToken = default);
    }
}
