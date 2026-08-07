using MultiShop.WebUI.Models.Catalog.CategoryDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.CategoryServices
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(CreateCategoryDto createCategoryDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PostAsJsonAsync("api/categories", createCategoryDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync("api/categories/" + categoryId, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<ResultCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultCategoryDto>>("api/categories", cancellationToken);
            if (result == null)
                return Array.Empty<ResultCategoryDto>();

            return result;
        }
    }
}
