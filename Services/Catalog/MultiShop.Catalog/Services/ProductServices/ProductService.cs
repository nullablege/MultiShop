using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.ProductDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public ProductService(IMongoDatabase database, IMapper mapper, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mapper = mapper;
            _productCollection = database.GetCollection<Product>(mongoDbSettings.Value.ProductCollectionName);
            _categoryCollection = database.GetCollection<Category>(
                mongoDbSettings.Value.CategoryCollectionName);
        }

        public async Task CreateAsync(CreateProductDto createProductDto, CancellationToken cancellationToken = default)
        {
            var value = _mapper.Map<Product>(createProductDto);
            await _productCollection.InsertOneAsync(value, options:null, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _productCollection.DeleteOneAsync(product => product.ProductId == id, options:null, cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<ResultProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var values = await _productCollection.Find(product => true).ToListAsync(cancellationToken);
            var result = _mapper.Map<IReadOnlyList<ResultProductDto>>(values);
            return result;
        }

        public async Task<GetByIdProductDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var value = await _productCollection.Find(product => product.ProductId == id, options:null).FirstOrDefaultAsync(cancellationToken);
            var result = _mapper.Map<GetByIdProductDto?>(value);
            return result;
        }

        public async Task<IReadOnlyList<ResultProductDto>> GetProductsByCategoryAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            var values = await _productCollection.Find(x => x.CategoryId == categoryId).ToListAsync(cancellationToken);
            var result = _mapper.Map<List<ResultProductDto>>(values);
            return result;
        }

        public async Task<IReadOnlyList<ResultProductWithCategoryDto>> GetWithCategoryAsync(CancellationToken cancellationToken = default)
        {
            var stages = new[]
            {
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", _categoryCollection.CollectionNamespace.CollectionName },
                    { "localField", "CategoryId" },
                    { "foreignField", "_id" },
                    { "as", "category" }
                }),
                new BsonDocument("$unwind", new BsonDocument
                {
                    { "path", "$category" },
                    { "preserveNullAndEmptyArrays", true }
                }),
                new BsonDocument("$project", new BsonDocument
                {
                    { "_id", 0 },
                    { "ProductId", new BsonDocument("$toString", "$_id") },
                    { "ProductName", 1 },
                    { "ProductPrice", 1 },
                    { "CoverImageUrl", 1 },
                    { "CategoryId", new BsonDocument("$toString", "$CategoryId") },
                    { "CategoryName", "$category.CategoryName" }
                })
            };

            var pipeline = PipelineDefinition<Product, ResultProductWithCategoryDto>.Create(stages);

            var values = await _productCollection
                .Aggregate(pipeline, cancellationToken: cancellationToken)
                .ToListAsync(cancellationToken);

            return values;
        }

        public async Task<bool> UpdateAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken = default)
        {
            var value = _mapper.Map<Product>(updateProductDto);
            var result = await _productCollection.ReplaceOneAsync(product => product.ProductId==value.ProductId , value, new ReplaceOptions(), cancellationToken);
            return result.MatchedCount > 0;
        }
    }
}
