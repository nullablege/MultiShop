using MultiShop.WebUI.Models.Catalog.BrandDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.BrandServices;

public sealed class BrandService : IBrandService
{
    private readonly HttpClient _httpClient;

    public BrandService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateAsync(CreateBrandDto createBrandDto, CancellationToken cancellationToken = default)
    {
        var result = await _httpClient.PostAsJsonAsync("api/brands", createBrandDto, cancellationToken);
        result.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string brandId, CancellationToken cancellationToken = default)
    {
        var result = await _httpClient.DeleteAsync("api/brands/" + brandId, cancellationToken);
        result.EnsureSuccessStatusCode();
    }

    public async Task<IReadOnlyList<ResultBrandDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await _httpClient.GetFromJsonAsync<List<ResultBrandDto>>("api/brands", cancellationToken);
        if (result == null)
            return Array.Empty<ResultBrandDto>();

        return result;
    }

    public async Task<UpdateBrandDto?> GetByIdAsync(string brandId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<UpdateBrandDto>("api/brands/" + brandId, cancellationToken);
    }

    public async Task UpdateAsync(UpdateBrandDto updateBrandDto, CancellationToken cancellationToken = default)
    {
        var result = await _httpClient.PutAsJsonAsync("api/brands/" + updateBrandDto.BrandId, updateBrandDto, cancellationToken);
        result.EnsureSuccessStatusCode();
    }
}
