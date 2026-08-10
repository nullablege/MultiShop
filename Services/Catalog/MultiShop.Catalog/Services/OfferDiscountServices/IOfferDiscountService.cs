using MultiShop.Catalog.DTOs.OfferDiscountDTOs;

namespace MultiShop.Catalog.Services.OfferDiscountServices
{
    public interface IOfferDiscountService
    {
        Task<IReadOnlyList<ResultOfferDiscountDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<GetByIdOfferDiscountDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateOfferDiscountDto createOfferDiscountDto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(UpdateOfferDiscountDto updateOfferDiscountDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
