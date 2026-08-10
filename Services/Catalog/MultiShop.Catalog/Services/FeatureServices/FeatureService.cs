using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.FeatureDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.FeatureServices
{
    public class FeatureService : IFeatureService
    {
        private readonly IMongoCollection<Feature> _featureCollection;
        private readonly IMapper _mapper;

        public FeatureService(IMongoDatabase database, IMapper mapper, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _featureCollection = database.GetCollection<Feature>(mongoDbSettings.Value.FeatureCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateFeatureDto createFeatureDto, CancellationToken cancellationToken = default)
        {
            var feature = _mapper.Map<Feature>(createFeatureDto);
            await _featureCollection.InsertOneAsync(feature, options: null, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _featureCollection.DeleteOneAsync(feature => feature.FeatureId == id, options: null, cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<ResultFeatureDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var features = await _featureCollection.Find(feature => true).ToListAsync(cancellationToken);
            return _mapper.Map<List<ResultFeatureDto>>(features);
        }

        public async Task<GetByIdFeatureDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var feature = await _featureCollection.Find(feature => feature.FeatureId == id).FirstOrDefaultAsync(cancellationToken);
            return _mapper.Map<GetByIdFeatureDto?>(feature);
        }

        public async Task<bool> UpdateAsync(UpdateFeatureDto updateFeatureDto, CancellationToken cancellationToken = default)
        {
            var feature = _mapper.Map<Feature>(updateFeatureDto);
            var result = await _featureCollection.ReplaceOneAsync(
                currentFeature => currentFeature.FeatureId == feature.FeatureId,
                feature,
                new ReplaceOptions(),
                cancellationToken);

            return result.MatchedCount > 0;
        }
    }
}
