using MongoDB.Bson;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.StatisticDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.StatisticServices;

public sealed class CatalogStatisticsService : ICatalogStatisticsService
{
    private readonly IMongoCollection<Brand> _brandCollection;
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMongoCollection<Product> _productCollection;

    public CatalogStatisticsService(
        IMongoDatabase database,
        Microsoft.Extensions.Options.IOptions<MongoDbSettings> settings)
    {
        var options = settings.Value;
        _brandCollection = database.GetCollection<Brand>(options.BrandCollectionName);
        _categoryCollection = database.GetCollection<Category>(options.CategoryCollectionName);
        _productCollection = database.GetCollection<Product>(options.ProductCollectionName);
    }

    public async Task<CatalogStatisticsDto> GetAsync(CancellationToken cancellationToken = default)
    {
        var brandCountTask = _brandCollection.CountDocumentsAsync(
            FilterDefinition<Brand>.Empty,
            cancellationToken: cancellationToken);
        var categoryCountTask = _categoryCollection.CountDocumentsAsync(
            FilterDefinition<Category>.Empty,
            cancellationToken: cancellationToken);
        var productCountTask = _productCollection.CountDocumentsAsync(
            FilterDefinition<Product>.Empty,
            cancellationToken: cancellationToken);
        var mostExpensiveProductTask = GetProductNameAsync(
            Builders<Product>.Sort.Descending(product => product.ProductPrice),
            cancellationToken);
        var leastExpensiveProductTask = GetProductNameAsync(
            Builders<Product>.Sort.Ascending(product => product.ProductPrice),
            cancellationToken);
        var averageProductPriceTask = GetAverageProductPriceAsync(cancellationToken);

        await Task.WhenAll(
            brandCountTask,
            categoryCountTask,
            productCountTask,
            mostExpensiveProductTask,
            leastExpensiveProductTask,
            averageProductPriceTask);

        return new CatalogStatisticsDto
        {
            BrandCount = await brandCountTask,
            CategoryCount = await categoryCountTask,
            ProductCount = await productCountTask,
            AverageProductPrice = await averageProductPriceTask,
            MostExpensiveProductName = await mostExpensiveProductTask,
            LeastExpensiveProductName = await leastExpensiveProductTask
        };
    }

    private async Task<string> GetProductNameAsync(
        SortDefinition<Product> sort,
        CancellationToken cancellationToken)
    {
        return await _productCollection
            .Find(FilterDefinition<Product>.Empty)
            .Sort(sort)
            .Project(product => product.ProductName)
            .FirstOrDefaultAsync(cancellationToken)
            ?? string.Empty;
    }

    private async Task<decimal> GetAverageProductPriceAsync(CancellationToken cancellationToken)
    {
        var result = await _productCollection.Aggregate()
            .Group(new BsonDocument
            {
                { "_id", BsonNull.Value },
                { "averagePrice", new BsonDocument("$avg", "$ProductPrice") }
            })
            .FirstOrDefaultAsync(cancellationToken);

        return result is null
            ? decimal.Zero
            : result.GetValue("averagePrice", BsonDecimal128.Create(decimal.Zero)).ToDecimal();
    }
}
