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
            var result = await _httpClient.PostAsJsonAsync("categories", createCategoryDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync("categories/" + categoryId, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<UpdateCategoryDto?> GetByIdAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<UpdateCategoryDto>("categories/" + categoryId, cancellationToken);
        }

        public async Task<IReadOnlyList<ResultCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultCategoryDto>>("categories", cancellationToken);
            if (result == null)
                return Array.Empty<ResultCategoryDto>();

            return result;
        }

        public async Task UpdateAsync(UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PutAsJsonAsync("categories/" + updateCategoryDto.CategoryId, updateCategoryDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }
    }
}
