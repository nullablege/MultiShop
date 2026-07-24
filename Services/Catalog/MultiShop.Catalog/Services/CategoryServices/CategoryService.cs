using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.CategoryDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.CategoryServices
{
    public class CategoryService:ICategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;

        public CategoryService(IMongoDatabase database, IOptions<MongoDbSettings> mongoDbSettings, IMapper mapper) {
            _mapper = mapper;
            _categoryCollection = database.GetCollection<Category>(mongoDbSettings.Value.CategoryCollectionName);
        }
        public async Task CreateAsync(CreateCategoryDto createCategoryDto, CancellationToken cancellationToken = default)
        {
            var value = _mapper.Map<Category>(createCategoryDto);
            await _categoryCollection.InsertOneAsync(value, options: null, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _categoryCollection.DeleteOneAsync(category => category.CategoryId == id , options: null, cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<ResultCategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var values = await _categoryCollection.Find(category => true).ToListAsync(cancellationToken);
            var result = _mapper.Map<List<ResultCategoryDto>>(values);

            return result;
        }

        public async Task<GetByIdCategoryDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var value = await _categoryCollection.Find(category => category.CategoryId==id).FirstOrDefaultAsync(cancellationToken);
            var result = _mapper.Map<GetByIdCategoryDto?>(value);
            return result;
        }

        public async Task<bool> UpdateAsync(UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken = default)
        {
            var element = _mapper.Map<Category>(updateCategoryDto);
            var result = await _categoryCollection.ReplaceOneAsync(category => category.CategoryId == element.CategoryId, element, new ReplaceOptions(), cancellationToken);
            return result.MatchedCount > 0;
        }
    }
}
