using MultiShop.WebUI.Models.Catalog.FeatureDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.FeatureServices
{
    public interface IFeatureService
    {
        Task<IReadOnlyList<ResultFeatureDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UpdateFeatureDto?> GetByIdAsync(string featureId, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateFeatureDto createFeatureDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateFeatureDto updateFeatureDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(string featureId, CancellationToken cancellationToken = default);
    }
}
