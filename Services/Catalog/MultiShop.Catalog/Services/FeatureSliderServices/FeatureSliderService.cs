using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.FeatureSliderDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.FeatureSliderServices
{
    public class FeatureSliderService : IFeatureSliderService
    {
        private readonly IMongoCollection<FeatureSlider> _featureCollection;
        private readonly IMapper _mapper;

        public FeatureSliderService(IMongoDatabase database, IMapper mapper, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _featureCollection = database.GetCollection<FeatureSlider>(mongoDbSettings.Value.FeatureSliderCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateFeatureSliderDto createFeatureSliderDto, CancellationToken cancellationToken = default)
        {
            var value = _mapper.Map<FeatureSlider>(createFeatureSliderDto);
            await _featureCollection.InsertOneAsync(value, options: null, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _featureCollection.DeleteOneAsync(x => x.FeatureSliderId == id, options: null, cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<ResultFeatureSliderDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var values = await _featureCollection.Find(x => true).ToListAsync(cancellationToken);
            var result = _mapper.Map<List<ResultFeatureSliderDto>>(values);
            return result;
        }

        public async Task<GetByIdFeatureSliderDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var value = await _featureCollection.Find(x => x.FeatureSliderId == id ).FirstOrDefaultAsync(cancellationToken);
            var result = _mapper.Map<GetByIdFeatureSliderDto?>(value);
            return result;
        }

        public async Task<bool> UpdateAsync(UpdateFeatureSliderDto updateFeatureSliderDto, CancellationToken cancellationToken = default)
        {
            var value = _mapper.Map<FeatureSlider>(updateFeatureSliderDto);
            var result = await _featureCollection.ReplaceOneAsync(x => x.FeatureSliderId == value.FeatureSliderId, value, new ReplaceOptions(), cancellationToken);
            return result.MatchedCount > 0;


        }
    }
}
