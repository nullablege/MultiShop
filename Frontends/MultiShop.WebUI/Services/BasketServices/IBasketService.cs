using MultiShop.WebUI.Models.Basket;

namespace MultiShop.WebUI.Services.BasketServices;

public interface IBasketService
{
    Task<BasketTotalDto> GetAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(BasketTotalDto basket, CancellationToken cancellationToken = default);
    Task AddItemAsync(BasketItemDto basketItem, CancellationToken cancellationToken = default);
    Task<bool> RemoveItemAsync(string productId, CancellationToken cancellationToken = default);
    Task DeleteAsync(CancellationToken cancellationToken = default);
}
