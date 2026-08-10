using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.OfferDiscountDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.OfferDiscountServices
{
    public class OfferDiscountService : IOfferDiscountService
    {
        private readonly IMongoCollection<OfferDiscount> _offerDiscountCollection;
        private readonly IMapper _mapper;

        public OfferDiscountService(IMongoDatabase database, IMapper mapper, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _offerDiscountCollection = database.GetCollection<OfferDiscount>(mongoDbSettings.Value.OfferDiscountCollectionName);
            _mapper = mapper;
        }

        public async Task CreateAsync(CreateOfferDiscountDto createOfferDiscountDto, CancellationToken cancellationToken = default)
        {
            var offerDiscount = _mapper.Map<OfferDiscount>(createOfferDiscountDto);
            await _offerDiscountCollection.InsertOneAsync(offerDiscount, options: null, cancellationToken);
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var result = await _offerDiscountCollection.DeleteOneAsync(offerDiscount => offerDiscount.OfferDiscountId == id, options: null, cancellationToken);
            return result.DeletedCount > 0;
        }

        public async Task<IReadOnlyList<ResultOfferDiscountDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var offerDiscounts = await _offerDiscountCollection.Find(offerDiscount => true).ToListAsync(cancellationToken);
            return _mapper.Map<List<ResultOfferDiscountDto>>(offerDiscounts);
        }

        public async Task<GetByIdOfferDiscountDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            var offerDiscount = await _offerDiscountCollection.Find(offerDiscount => offerDiscount.OfferDiscountId == id).FirstOrDefaultAsync(cancellationToken);
            return _mapper.Map<GetByIdOfferDiscountDto?>(offerDiscount);
        }

        public async Task<bool> UpdateAsync(UpdateOfferDiscountDto updateOfferDiscountDto, CancellationToken cancellationToken = default)
        {
            var offerDiscount = _mapper.Map<OfferDiscount>(updateOfferDiscountDto);
            var result = await _offerDiscountCollection.ReplaceOneAsync(currentOfferDiscount => currentOfferDiscount.OfferDiscountId == offerDiscount.OfferDiscountId, offerDiscount, new ReplaceOptions(), cancellationToken);
            return result.MatchedCount > 0;
        }
    }
}
