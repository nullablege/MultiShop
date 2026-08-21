using MultiShop.WebUI.Models.Catalog.AboutDTOs;
namespace MultiShop.WebUI.Services.CatalogServices.AboutServices;
public sealed class AboutService : IAboutService
{
    private readonly HttpClient _httpClient;
    public AboutService(HttpClient httpClient) { _httpClient = httpClient; }
    public async Task<IReadOnlyList<ResultAboutDto>> GetAllAsync(CancellationToken cancellationToken = default) { var result = await _httpClient.GetFromJsonAsync<List<ResultAboutDto>>("abouts", cancellationToken); if (result == null) return Array.Empty<ResultAboutDto>(); return result; }
    public Task<UpdateAboutDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default) => _httpClient.GetFromJsonAsync<UpdateAboutDto>("abouts/" + id, cancellationToken);
    public async Task CreateAsync(CreateAboutDto dto, CancellationToken cancellationToken = default) { var result = await _httpClient.PostAsJsonAsync("abouts", dto, cancellationToken); result.EnsureSuccessStatusCode(); }
    public async Task UpdateAsync(UpdateAboutDto dto, CancellationToken cancellationToken = default) { var result = await _httpClient.PutAsJsonAsync("abouts/" + dto.AboutId, dto, cancellationToken); result.EnsureSuccessStatusCode(); }
    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default) { var result = await _httpClient.DeleteAsync("abouts/" + id, cancellationToken); result.EnsureSuccessStatusCode(); }
}
