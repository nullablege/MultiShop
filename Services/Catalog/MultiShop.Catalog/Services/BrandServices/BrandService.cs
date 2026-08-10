using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.BrandDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.BrandServices;

public class BrandService : IBrandService
{
    private readonly IMongoCollection<Brand> _brandCollection;
    private readonly IMapper _mapper;

    public BrandService(IMongoDatabase database, IMapper mapper, IOptions<MongoDbSettings> mongoDbSettings)
    {
        _brandCollection = database.GetCollection<Brand>(mongoDbSettings.Value.BrandCollectionName);
        _mapper = mapper;
    }

    public async Task CreateAsync(CreateBrandDto createBrandDto, CancellationToken cancellationToken = default)
    {
        await _brandCollection.InsertOneAsync(_mapper.Map<Brand>(createBrandDto), options: null, cancellationToken);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await _brandCollection.DeleteOneAsync(brand => brand.BrandId == id, options: null, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<IReadOnlyList<ResultBrandDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var brands = await _brandCollection.Find(brand => true).ToListAsync(cancellationToken);
        return _mapper.Map<List<ResultBrandDto>>(brands);
    }

    public async Task<GetByIdBrandDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var brand = await _brandCollection.Find(brand => brand.BrandId == id).FirstOrDefaultAsync(cancellationToken);
        return _mapper.Map<GetByIdBrandDto?>(brand);
    }

    public async Task<bool> UpdateAsync(UpdateBrandDto updateBrandDto, CancellationToken cancellationToken = default)
    {
        var brand = _mapper.Map<Brand>(updateBrandDto);
        var result = await _brandCollection.ReplaceOneAsync(currentBrand => currentBrand.BrandId == brand.BrandId, brand, new ReplaceOptions(), cancellationToken);
        return result.MatchedCount > 0;
    }
}
