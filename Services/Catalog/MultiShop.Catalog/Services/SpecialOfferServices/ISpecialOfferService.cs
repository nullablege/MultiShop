using MultiShop.Catalog.DTOs.SpecialOfferDTOs;

namespace MultiShop.Catalog.Services.SpecialOfferServices
{
    public interface ISpecialOfferService
    {
        Task<IReadOnlyList<ResultSpecialOfferDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<GetByIdSpecialOfferDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateSpecialOfferDto createSpecialOfferDto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(UpdateSpecialOfferDto updateSpecialOfferDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
