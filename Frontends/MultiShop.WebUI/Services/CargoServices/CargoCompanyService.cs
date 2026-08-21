using System.Net;
using MultiShop.WebUI.Models.CargoDTOs;

namespace MultiShop.WebUI.Services.CargoServices;

public sealed class CargoCompanyService : ICargoCompanyService
{
    private readonly HttpClient _httpClient;

    public CargoCompanyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<ResultCargoCompanyDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var cargoCompanies = await _httpClient.GetFromJsonAsync<List<ResultCargoCompanyDto>>(
            "cargocompanies",
            cancellationToken);

        return cargoCompanies is null
            ? Array.Empty<ResultCargoCompanyDto>()
            : cargoCompanies;
    }

    public async Task<UpdateCargoCompanyDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(
            $"cargocompanies/{id}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UpdateCargoCompanyDto>(
            cancellationToken);
    }

    public async Task CreateAsync(
        CreateCargoCompanyDto createCargoCompanyDto,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "cargocompanies",
            createCargoCompanyDto,
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAsync(
        UpdateCargoCompanyDto updateCargoCompanyDto,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PutAsJsonAsync(
            $"cargocompanies/{updateCargoCompanyDto.CargoCompanyId}",
            updateCargoCompanyDto,
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.DeleteAsync(
            $"cargocompanies/{id}",
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
