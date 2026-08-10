using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MultiShop.Catalog.Authorization;
using MultiShop.Catalog.Mapping;
using MultiShop.Catalog.Services.CategoryServices;
using MultiShop.Catalog.Services.FeatureSliderServices;
using MultiShop.Catalog.Services.FeatureServices;
using MultiShop.Catalog.Services.ProductServices;
using MultiShop.Catalog.Services.SpecialOfferServices;
using MultiShop.Catalog.Services.OfferDiscountServices;
using MultiShop.Catalog.Services.BrandServices;
using MultiShop.Catalog.Settings;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var identityProviderOptions = builder.Configuration
    .GetRequiredSection(IdentityProviderOptions.SectionName)
    .Get<IdentityProviderOptions>()
    ?? throw new InvalidOperationException(
        "IdentityProvider yapılandırması bulunamadı.");

if (!Uri.TryCreate(
        identityProviderOptions.Issuer,
        UriKind.Absolute,
        out var identityProviderIssuer)
    || identityProviderIssuer.Scheme != Uri.UriSchemeHttps)
{
    throw new InvalidOperationException(
        "IdentityProvider:Issuer geçerli bir HTTPS adresi olmalıdır.");
}

builder.Services.AddControllers();

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer(identityProviderIssuer);
        options.AddAudiences(CatalogAuthorizationConstants.Audience);
        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        CatalogAuthorizationConstants.Policy,
        policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireAssertion(context =>
                context.User.HasScope(
                    CatalogAuthorizationConstants.Scope));
        });
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
            settings => !string.IsNullOrWhiteSpace(settings.FeatureSliderCollectionName),
            "MongoDb:FeatureSliderCollectionName zorunludur.")
        .Validate(
            settings => !string.IsNullOrWhiteSpace(settings.SpecialOfferCollectionName),
            "MongoDb:SpecialOfferCollectionName zorunludur.")
        .Validate(
            settings => !string.IsNullOrWhiteSpace(settings.OfferDiscountCollectionName),
            "MongoDb:OfferDiscountCollectionName zorunludur.")
        .Validate(
            settings => !string.IsNullOrWhiteSpace(settings.BrandCollectionName),
            "MongoDb:BrandCollectionName zorunludur.")
        .Validate(
            settings => !string.IsNullOrWhiteSpace(settings.FeatureCollectionName),
            "MongoDb:FeatureCollectionName zorunludur.")
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
builder.Services.AddScoped<IFeatureSliderService, FeatureSliderService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISpecialOfferService, SpecialOfferService>();
builder.Services.AddScoped<IOfferDiscountService, OfferDiscountService>();
builder.Services.AddScoped<IBrandService, BrandService>();


var app = builder.Build();

//Devlopment sırasında uygunsuz mapping kontrolü
if (app.Environment.IsDevelopment())
{
    app.Services
        .GetRequiredService<AutoMapper.IConfigurationProvider>()
        .AssertConfigurationIsValid();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
