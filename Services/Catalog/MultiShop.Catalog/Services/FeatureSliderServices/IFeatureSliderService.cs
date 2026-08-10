using MultiShop.Catalog.DTOs.FeatureSliderDTOs;

namespace MultiShop.Catalog.Services.FeatureSliderServices
{
    public interface IFeatureSliderService
    {
        Task<IReadOnlyList<ResultFeatureSliderDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<GetByIdFeatureSliderDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateFeatureSliderDto createFeatureSliderDto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(UpdateFeatureSliderDto updateFeatureSliderDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
