using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.SpecialOfferDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.SpecialOfferServices
{
    public class SpecialOfferService : ISpecialOfferService
    {
        private readonly IMongoCollection<SpecialOffer> _specialOfferCollection;
        private readonly IMapper _mapper;

        public SpecialOfferService(IMongoDatabase database, IMapper mapper, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _specialOfferCollection = database.GetCollection<SpecialOffer>(mongoDbSettings.Value.SpecialOfferCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateSpecialOfferDto createSpecialOfferDto, CancellationToken cancellationToken = default)
        {
            var specialOffer = _mapper.Map<SpecialOffer>(createSpecialOfferDto);
            await _specialOfferCollection.InsertOneAsync(specialOffer, options: null, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _specialOfferCollection.DeleteOneAsync(
                specialOffer => specialOffer.SpecialOfferId == id,
                options: null,
                cancellationToken);

            return result.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<ResultSpecialOfferDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var specialOffers = await _specialOfferCollection.Find(specialOffer => true).ToListAsync(cancellationToken);
            return _mapper.Map<List<ResultSpecialOfferDto>>(specialOffers);
        }

        public async Task<GetByIdSpecialOfferDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var specialOffer = await _specialOfferCollection
                .Find(specialOffer => specialOffer.SpecialOfferId == id)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<GetByIdSpecialOfferDto?>(specialOffer);
        }

        public async Task<bool> UpdateAsync(UpdateSpecialOfferDto updateSpecialOfferDto, CancellationToken cancellationToken = default)
        {
            var specialOffer = _mapper.Map<SpecialOffer>(updateSpecialOfferDto);
            var result = await _specialOfferCollection.ReplaceOneAsync(
                currentSpecialOffer => currentSpecialOffer.SpecialOfferId == specialOffer.SpecialOfferId,
                specialOffer,
                new ReplaceOptions(),
                cancellationToken);

            return result.MatchedCount > 0;
        }
    }
}
