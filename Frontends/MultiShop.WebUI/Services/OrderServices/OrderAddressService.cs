using MultiShop.WebUI.Models.Order;

namespace MultiShop.WebUI.Services.OrderServices;

public sealed class OrderAddressService : IOrderAddressService
{
    private readonly HttpClient _httpClient;

    public OrderAddressService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CreateAsync(CreateOrderAddressDto createOrderAddressDto, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("addresses", createOrderAddressDto, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
