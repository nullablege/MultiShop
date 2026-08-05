using Microsoft.Extensions.Options;
using MultiShop.Basket.Configuration;
using StackExchange.Redis;

namespace MultiShop.Basket.Services
{
    public sealed class RedisConnectionProvider
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly RedisOptions _redisOptions;


        public RedisConnectionProvider(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisOptions> redisOptions)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _redisOptions = redisOptions.Value;
        }
        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase(_redisOptions.Database);
        }


    }
}
