using MultiShop.WebUI.Models.Catalog.FeatureDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.FeatureServices
{
    public sealed class FeatureService : IFeatureService
    {
        private readonly HttpClient _httpClient;

        public FeatureService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(CreateFeatureDto createFeatureDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PostAsJsonAsync("api/features", createFeatureDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string featureId, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync("api/features/" + featureId, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<ResultFeatureDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultFeatureDto>>("api/features", cancellationToken);
            if (result == null)
                return Array.Empty<ResultFeatureDto>();

            return result;
        }

        public async Task<UpdateFeatureDto?> GetByIdAsync(string featureId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<UpdateFeatureDto>("api/features/" + featureId, cancellationToken);
        }

        public async Task UpdateAsync(UpdateFeatureDto updateFeatureDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PutAsJsonAsync(
                "api/features/" + updateFeatureDto.FeatureId,
                updateFeatureDto,
                cancellationToken);

            result.EnsureSuccessStatusCode();
        }
    }
}
