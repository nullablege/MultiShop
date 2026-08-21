using System.Net;
using MultiShop.WebUI.Models.CargoDTOs;

namespace MultiShop.WebUI.Services.CargoServices;

public sealed class CargoCustomerService : ICargoCustomerService
{
    private readonly HttpClient _httpClient;

    public CargoCustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CargoCustomerDetailDto?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.GetAsync(
            $"cargocustomers/by-user/{Uri.EscapeDataString(userId)}",
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CargoCustomerDetailDto>(
            cancellationToken);
    }
}
