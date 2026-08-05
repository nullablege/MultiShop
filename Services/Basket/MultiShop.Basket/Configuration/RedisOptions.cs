namespace MultiShop.Basket.Configuration
{
    public sealed class RedisOptions
    {
        public const string SectionName = "Redis";
        public string ConnectionString { get; init; } = string.Empty;
        public int Database { get; init; }
        public int BasketTtlDays { get; init; } = 7;
    }
}
