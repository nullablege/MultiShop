using Microsoft.Extensions.Options;
using MultiShop.Basket.Configuration;
using MultiShop.Basket.Dtos;
using StackExchange.Redis;
using System.Text.Json;

namespace MultiShop.Basket.Services
{
    public sealed class BasketService : IBasketService
    {
        private readonly IDatabase _database;
        private readonly TimeSpan _basketTtl;
        public BasketService(RedisConnectionProvider redisConnectionProvider, IOptions<RedisOptions> redisOptions)
        {
            _database = redisConnectionProvider.GetDatabase();
            _basketTtl = TimeSpan.FromDays(redisOptions.Value.BasketTtlDays);
        }

        public async Task<bool> DeleteBasketAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("Kullanıcı ID boş olamaz.", nameof(userId));

            var key = GetBasketKey(userId);
            return await _database.KeyDeleteAsync(key).WaitAsync(cancellationToken);
        }

        public async Task<BasketTotalDto?> GetBasketAsync(string userId, CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("Kullanıcı ID boş olamaz", nameof(userId));

            var key = GetBasketKey(userId);
            var basketJson = await _database.StringGetAsync(key).WaitAsync(cancellationToken);

            if (basketJson.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<BasketTotalDto>(basketJson.ToString());

        }

        public async Task<bool> SaveBasketAsync(BasketTotalDto basketTotalDto, CancellationToken cancellationToken = default)
        {
            if(basketTotalDto == null)
                throw new ArgumentNullException(nameof(basketTotalDto));

            if(string.IsNullOrWhiteSpace(basketTotalDto.UserId))
                throw new ArgumentException("Kullanıcı ID boş olamaz", nameof(basketTotalDto));

            var key = GetBasketKey(basketTotalDto.UserId);
            var basketJson = JsonSerializer.Serialize(basketTotalDto);
            return await _database.StringSetAsync(key, basketJson, _basketTtl).WaitAsync(cancellationToken);
        }

        private static string GetBasketKey(string userId)
        {
            return $"basket:{userId}";
        }
    }
}
