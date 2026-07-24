using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.Mapping;
using MultiShop.Catalog.Services.CategoryServices;
using MultiShop.Catalog.Services.ProductServices;
using MultiShop.Catalog.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
        .AddOptions<MongoDbSettings>()
        .Bind(builder.Configuration.GetSection(MongoDbSettings.SectionName))
        .Validate(
                  settings => !string.IsNullOrWhiteSpace(settings.DatabaseName),
                  "MongoDb:Database zorunludur.")
        .Validate(
            settings => !string.IsNullOrWhiteSpace(settings.CategoryCollectionName),
                "MongoDb:CategoryCollectionName zorunludur.")
        .Validate(
            settings => !string.IsNullOrWhiteSpace(settings.ProductCollectionName),
            "MongoDb:ProductCollectionName zorunludur.")
        .Validate(
            settings => !string.IsNullOrWhiteSpace(settings.ConnectionString),
            "MongoDb:ConnectionString zorunludur.")
        .ValidateOnStart();

builder.Services.AddAutoMapper(_ => { }, typeof(CatalogMappingProfile));

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var mongoDbSettings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;

    return new MongoClient(mongoDbSettings.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(serviceProvider =>
{
    var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();

    var mongoDbSettings = serviceProvider
        .GetRequiredService<IOptions<MongoDbSettings>>()
        .Value;

    return mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
});


builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();

//Devlopment sýrasýnda uygunsuz mapping kontrolü
if (app.Environment.IsDevelopment())
{
    app.Services
        .GetRequiredService<AutoMapper.IConfigurationProvider>()
        .AssertConfigurationIsValid();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
