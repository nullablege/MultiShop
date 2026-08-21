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
            var result = await _httpClient.PostAsJsonAsync("products", createProductDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string productId, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync("products/" + productId, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<ResultProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultProductDto>>("products", cancellationToken);
            if (result == null)
                return Array.Empty<ResultProductDto>();

            return result;
        }

        public async Task<IReadOnlyList<ResultProductWithCategoryDto>> GetWithCategoryAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultProductWithCategoryDto>>("products/with-category", cancellationToken);
            if (result == null)
                return Array.Empty<ResultProductWithCategoryDto>();

            return result;
        }

        public async Task<GetByIdProductDto?> GetByIdAsync(string productId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<GetByIdProductDto>("products/" + productId, cancellationToken);
        }

        public async Task UpdateAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PutAsJsonAsync("products/" + updateProductDto.ProductId, updateProductDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<ResultProductDto>> GetByCategoryIdAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var result =  await _httpClient.GetFromJsonAsync<List<ResultProductDto>>("products/by-category/" + categoryId, cancellationToken);
            if(result == null)
                return Array.Empty<ResultProductDto>();

            return result;
        }

        public async Task<UpdateProductDto?> GetForUpdateAsync(string productId, CancellationToken cancellation = default)
        {
            var result = await _httpClient.GetFromJsonAsync<UpdateProductDto>("products/"+productId, cancellation);
            return result;
        }
    }
}
