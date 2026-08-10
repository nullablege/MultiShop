using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.AboutDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.AboutServices;

public class AboutService : IAboutService
{
    private readonly IMongoCollection<About> _aboutCollection;
    private readonly IMapper _mapper;

    public AboutService(IMongoDatabase database, IMapper mapper, IOptions<MongoDbSettings> mongoDbSettings)
    {
        _aboutCollection = database.GetCollection<About>(mongoDbSettings.Value.AboutCollectionName);
        _mapper = mapper;
    }

    public async Task CreateAsync(CreateAboutDto createAboutDto, CancellationToken cancellationToken = default)
    {
        await _aboutCollection.InsertOneAsync(_mapper.Map<About>(createAboutDto), options: null, cancellationToken);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await _aboutCollection.DeleteOneAsync(about => about.AboutId == id, options: null, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<IReadOnlyList<ResultAboutDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var abouts = await _aboutCollection.Find(about => true).ToListAsync(cancellationToken);
        return _mapper.Map<List<ResultAboutDto>>(abouts);
    }

    public async Task<GetByIdAboutDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var about = await _aboutCollection.Find(about => about.AboutId == id).FirstOrDefaultAsync(cancellationToken);
        return _mapper.Map<GetByIdAboutDto?>(about);
    }

    public async Task<bool> UpdateAsync(UpdateAboutDto updateAboutDto, CancellationToken cancellationToken = default)
    {
        var about = _mapper.Map<About>(updateAboutDto);
        var result = await _aboutCollection.ReplaceOneAsync(currentAbout => currentAbout.AboutId == about.AboutId, about, new ReplaceOptions(), cancellationToken);
        return result.MatchedCount > 0;
    }
}
