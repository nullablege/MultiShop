using System.Net;
using MultiShop.WebUI.Models.Basket;

namespace MultiShop.WebUI.Services.BasketServices;

public sealed class BasketService : IBasketService
{
    private readonly HttpClient _httpClient;

    public BasketService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BasketTotalDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("baskets", cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new BasketTotalDto();
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BasketTotalDto>(cancellationToken) ?? new BasketTotalDto();
    }

    public async Task SaveAsync(BasketTotalDto basket, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync("baskets", basket, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task AddItemAsync(BasketItemDto basketItem, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(basketItem);

        var basket = await GetAsync(cancellationToken);
        var existingItem = basket.BasketItems.FirstOrDefault(item => item.ProductId == basketItem.ProductId);

        if (existingItem is null)
        {
            basket.BasketItems.Add(basketItem);
        }
        else
        {
            existingItem.Quantity += basketItem.Quantity;
        }

        await SaveAsync(basket, cancellationToken);
    }

    public async Task<bool> RemoveItemAsync(string productId, CancellationToken cancellationToken = default)
    {
        var basket = await GetAsync(cancellationToken);
        var item = basket.BasketItems.FirstOrDefault(value => value.ProductId == productId);
        if (item is null)
        {
            return false;
        }

        basket.BasketItems.Remove(item);
        if (basket.BasketItems.Count == 0)
        {
            await DeleteAsync(cancellationToken);
        }
        else
        {
            await SaveAsync(basket, cancellationToken);
        }

        return true;
    }

    public async Task DeleteAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync("baskets", cancellationToken);
        if (response.StatusCode != HttpStatusCode.NotFound)
        {
            response.EnsureSuccessStatusCode();
        }
    }
}
