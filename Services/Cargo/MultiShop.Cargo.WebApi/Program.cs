using Microsoft.EntityFrameworkCore;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.BusinessLayer.Concrete;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Context;
using MultiShop.Cargo.DataAccessLayer.EntityFramework;
using MultiShop.Cargo.WebApi.Authorization;
using MultiShop.Cargo.WebApi.Configuration;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("CargoConnection")
    ?? throw new InvalidOperationException("Cargo veritabanı bağlantı metni bulunamadı.");

builder.Services.AddDbContext<CargoContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<ICargoCompanyDal, EfCargoCompanyDal>();
builder.Services.AddScoped<ICargoCustomerDal, EfCargoCustomerDal>();
builder.Services.AddScoped<ICargoDetailDal, EfCargoDetailDal>();
builder.Services.AddScoped<ICargoOperationDal, EfCargoOperationDal>();

builder.Services
    .AddOptions<IdentityProviderOptions>()
    .Bind(builder.Configuration.GetSection(
        IdentityProviderOptions.SectionName))
    .Validate(options =>
    {
        if (!Uri.TryCreate(
                options.Issuer,
                UriKind.Absolute,
                out var issuerUri))
        {
            return false;
        }

        return issuerUri.Scheme == Uri.UriSchemeHttp ||
               issuerUri.Scheme == Uri.UriSchemeHttps;
    }, "Cargo Issuer Hatalı")
    .ValidateOnStart();

var identityProviderOptions = builder.Configuration
    .GetRequiredSection(IdentityProviderOptions.SectionName)
    .Get<IdentityProviderOptions>()
    ?? throw new InvalidOperationException(
        "Cargo Identity Ayarı Hatalı");

builder.Services.AddOpenIddict()
    .AddValidation(validationOptions =>
    {
        validationOptions.SetIssuer(
            new Uri(identityProviderOptions.Issuer));

        validationOptions.AddAudiences(
            CargoAuthorizationConstants.Audience);

        validationOptions.UseSystemNetHttp();
        validationOptions.UseAspNetCore();
    });

builder.Services.AddAuthentication(authenticationOptions =>
{
    authenticationOptions.DefaultScheme =
        OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy(
        CargoAuthorizationConstants.AccessPolicy,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.RequireAssertion(context =>
                context.User.HasScope(
                    CargoAuthorizationConstants.Scope));
        });

    authorizationOptions.AddPolicy(
        CargoAuthorizationConstants.ManagementPolicy,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.RequireAssertion(context =>
                context.User.HasScope(
                    CargoAuthorizationConstants.Scope));

            policy.RequireRole(
                CargoAuthorizationConstants.AdminRole,
                CargoAuthorizationConstants.ManagerRole);
        });
});

builder.Services.AddScoped<ICargoCompanyService, CargoCompanyManager>();
builder.Services.AddScoped<ICargoCustomerService, CargoCustomerManager>();
builder.Services.AddScoped<ICargoDetailService, CargoDetailManager>();
builder.Services.AddScoped<ICargoOperationService, CargoOperationManager>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
