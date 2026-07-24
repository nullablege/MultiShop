using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.ProductDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _mongoCollection;
        private readonly IMapper _mapper;

        public ProductService(IMongoDatabase database, IMapper mapper, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mapper = mapper;
            _mongoCollection = database.GetCollection<Product>(mongoDbSettings.Value.ProductCollectionName);
        }

        public async Task CreateAsync(CreateProductDto createProductDto, CancellationToken cancellationToken = default)
        {
            var value = _mapper.Map<Product>(createProductDto);
            await _mongoCollection.InsertOneAsync(value, options:null, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _mongoCollection.DeleteOneAsync(product => product.ProductId == id, options:null, cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<ResultProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var values = await _mongoCollection.Find(product => true).ToListAsync(cancellationToken);
            var result = _mapper.Map<IReadOnlyList<ResultProductDto>>(values);
            return result;
        }

        public async Task<GetByIdProductDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var value = await _mongoCollection.Find(product => product.ProductId == id, options:null).FirstOrDefaultAsync(cancellationToken);
            var result = _mapper.Map<GetByIdProductDto?>(value);
            return result;
        }

        public async Task<bool> UpdateAsync(UpdateProductDto updateProductDto, CancellationToken cancellationToken = default)
        {
            var value = _mapper.Map<Product>(updateProductDto);
            var result = await _mongoCollection.ReplaceOneAsync(product => product.ProductId==value.ProductId , value, new ReplaceOptions(), cancellationToken);
            return result.MatchedCount > 0;
        }
    }
}
