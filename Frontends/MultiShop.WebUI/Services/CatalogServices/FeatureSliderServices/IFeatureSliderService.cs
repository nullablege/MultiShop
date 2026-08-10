using MultiShop.WebUI.Models.Catalog.FeatureSliderDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices
{
    public interface IFeatureSliderService
    {
        Task<IReadOnlyList<ResultFeatureSliderDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UpdateFeatureSliderDto?> GetByIdAsync(string featureSliderId, CancellationToken cancellationToken = default);
        Task CreateAsync(CreateFeatureSliderDto createFeatureSliderDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(UpdateFeatureSliderDto updateFeatureSliderDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(string featureSliderId, CancellationToken cancellationToken = default);
    }
}
