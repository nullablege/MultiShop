using MultiShop.Basket.Dtos;

namespace MultiShop.Basket.Services
{
    public interface IBasketService
    {
        Task<BasketTotalDto?> GetBasketAsync(string userId, CancellationToken cancellationToken = default);
        Task<bool> SaveBasketAsync(BasketTotalDto basketTotalDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteBasketAsync(string userId, CancellationToken cancellationToken = default);
    }
}
