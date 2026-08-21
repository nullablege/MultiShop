using MultiShop.WebUI.Models.Order;

namespace MultiShop.WebUI.Services.OrderServices;

public sealed class OrderHistoryService : IOrderHistoryService
{
    private readonly HttpClient _httpClient;

    public OrderHistoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<OrderHistoryItemDto>> GetCurrentUserOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _httpClient.GetFromJsonAsync<List<OrderHistoryItemDto>>("orderings/me", cancellationToken);
        return orders ?? [];
    }
}
