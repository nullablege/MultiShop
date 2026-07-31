using Microsoft.EntityFrameworkCore;
using MultiShop.Order.Application.Features.CQRS.Handlers.AddressHandlers;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Persistence.Context;
using MultiShop.Order.Persistence.Repositories;
using MultiShop.Order.WebApi.Authorization;
using MultiShop.Order.WebApi.Configuration;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    }, "Issuer adresi hatalý")
    .ValidateOnStart();

var identityProviderOptions = builder.Configuration
    .GetRequiredSection(IdentityProviderOptions.SectionName)
    .Get<IdentityProviderOptions>()
    ?? throw new InvalidOperationException(
        "Identity configuration dosyasý bulunamadý");

builder.Services.AddOpenIddict()
    .AddValidation(validationOptions =>
    {
        validationOptions.SetIssuer(
            new Uri(identityProviderOptions.Issuer));

        validationOptions.AddAudiences(
            OrderAuthorizationConstants.Audience);

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
        OrderAuthorizationConstants.AccessPolicy,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.RequireAssertion(context =>
                context.User.HasScope(
                    OrderAuthorizationConstants.Scope));
        });

    authorizationOptions.AddPolicy(
        OrderAuthorizationConstants.ManagementPolicy,
        policy =>
        {
            policy.RequireAuthenticatedUser();

            policy.RequireAssertion(context =>
                context.User.HasScope(
                    OrderAuthorizationConstants.Scope));

            policy.RequireRole(
                OrderAuthorizationConstants.AdminRole,
                OrderAuthorizationConstants.ManagerRole);
        });
});

var connectionString = builder.Configuration.GetConnectionString("OrderDb");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Order db connection string eksik");
}

builder.Services.AddDbContext<OrderDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblyContaining<GetAddressQueryHandler>();
});

var app = builder.Build();

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
