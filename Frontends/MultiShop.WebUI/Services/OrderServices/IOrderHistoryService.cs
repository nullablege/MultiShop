using MultiShop.WebUI.Models.Order;

namespace MultiShop.WebUI.Services.OrderServices;

public interface IOrderHistoryService
{
    Task<IReadOnlyList<OrderHistoryItemDto>> GetCurrentUserOrdersAsync(CancellationToken cancellationToken = default);
}
