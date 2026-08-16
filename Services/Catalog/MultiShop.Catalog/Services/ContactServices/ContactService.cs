using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.DTOs.ContactDTOs;
using MultiShop.Catalog.Entities;
using MultiShop.Catalog.Settings;

namespace MultiShop.Catalog.Services.ContactServices
{
    public class ContactService : IContactService
    {
        private readonly IMongoCollection<Contact> _contactCollection;
        private readonly IMapper _mapper;
        public ContactService(IMongoDatabase database, IOptions<MongoDbSettings> mongoDbSettings, IMapper mapper) { 
            _mapper = mapper;
            _contactCollection = database.GetCollection<Contact>(mongoDbSettings.Value.ContactCollectionName);
        }

        public async Task CreateAsync(CreateContactDto createContactDto, CancellationToken cancellationToken = default)
        {
            var value = _mapper.Map<Contact>(createContactDto);
            value.IsRead = false;
            value.CreatedAt = DateTime.UtcNow;
            await _contactCollection.InsertOneAsync(value,options: null, cancellationToken);
        }


    }
}
