using MultiShop.Catalog.DTOs.FeatureDTOs;

namespace MultiShop.Catalog.Services.FeatureServices
{
    public interface IFeatureService
    {
        Task<IReadOnlyList<ResultFeatureDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<GetByIdFeatureDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateFeatureDto createFeatureDto, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(UpdateFeatureDto updateFeatureDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
