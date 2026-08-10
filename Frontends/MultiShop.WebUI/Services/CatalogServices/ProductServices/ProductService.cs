using MultiShop.WebUI.Models.Catalog.ProductDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.ProductServices
{
    public sealed class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(CreateProductDto createProductDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PostAsJsonAsync("api/products", createProductDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string productId, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync("api/products/" + productId, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<ResultProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultProductDto>>("api/products", cancellationToken);
            if (result == null)
                return Array.Empty<ResultProductDto>();

            return result;
        }

        public async Task<IReadOnlyList<ResultProductWithCategoryDto>> GetWithCategoryAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultProductWithCategoryDto>>("api/products/with-category", cancellationToken);
            if (result == null)
                return Array.Empty<ResultProductWithCategoryDto>();

            return result;
        }

        public async Task<UpdateProductDto?> GetByIdAsync(string productId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<UpdateProductDto>("api/products/" + productId, cancellationToken);
        }

        public async Task UpdateAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PutAsJsonAsync("api/products/" + updateProductDto.ProductId, updateProductDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<ResultProductDto>> GetByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var result =  await _httpClient.GetFromJsonAsync<List<ResultProductDto>>("api/products/by-category/" + categoryId, cancellationToken);
            if(result == null)
                return Array.Empty<ResultProductDto>();

            return result;
        }
    }
}
