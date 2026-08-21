using MultiShop.WebUI.Models.Catalog.FeatureSliderDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices
{
    public sealed class FeatureSliderService : IFeatureSliderService
    {
        private readonly HttpClient _httpClient;

        public FeatureSliderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(CreateFeatureSliderDto createFeatureSliderDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PostAsJsonAsync("featuresliders", createFeatureSliderDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string featureSliderId, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync("featuresliders/" + featureSliderId, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<ResultFeatureSliderDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultFeatureSliderDto>>("featuresliders", cancellationToken);
            if (result == null)
                return Array.Empty<ResultFeatureSliderDto>();

            return result;
        }

        public async Task<UpdateFeatureSliderDto?> GetByIdAsync(string featureSliderId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<UpdateFeatureSliderDto>("featuresliders/" + featureSliderId, cancellationToken);
        }

        public async Task UpdateAsync(UpdateFeatureSliderDto updateFeatureSliderDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PutAsJsonAsync(
                "featuresliders/" + updateFeatureSliderDto.FeatureSliderId,
                updateFeatureSliderDto,
                cancellationToken);

            result.EnsureSuccessStatusCode();
        }
    }
}
