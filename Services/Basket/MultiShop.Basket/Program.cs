using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MultiShop.Basket.Authorization;
using MultiShop.Basket.Configuration;
using MultiShop.Basket.Services;
using MultiShop.Basket.Services.CurrentUser;
using MultiShop.Basket.Settings;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<RedisOptions>()
    .Bind(builder.Configuration.GetSection(RedisOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString),
        "Redis baðlantý adresi bulunamadý.")
    .Validate(options => options.Database >= 0,
        "Redis database numarasý negatif olamaz.")
    .Validate(options => options.BasketTtlDays > 0,
        "Sepet yaþam süresi sýfýrdan büyük olmalýdýr.")
    .ValidateOnStart();

builder.Services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
{
    var redisOptions = serviceProvider
        .GetRequiredService<IOptions<RedisOptions>>()
        .Value;

    return ConnectionMultiplexer.Connect(redisOptions.ConnectionString);
});

builder.Services.AddSingleton<RedisConnectionProvider>();

builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

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
    }, "Identity issuer ayarý hatalý")
    .ValidateOnStart();

var identityProviderOptions = builder.Configuration
    .GetRequiredSection(IdentityProviderOptions.SectionName)
    .Get<IdentityProviderOptions>()
    ?? throw new InvalidOperationException(
        "Identity ayarý bulunamadý");

builder.Services.AddOpenIddict()
    .AddValidation(validationOptions =>
    {
        validationOptions.SetIssuer(
            new Uri(identityProviderOptions.Issuer));

        validationOptions.AddAudiences(
            BasketAuthorizationConstants.Audience);

        validationOptions.UseSystemNetHttp();
        validationOptions.UseAspNetCore();
    });

builder.Services.AddAuthentication(authenticationOptions =>
{
    authenticationOptions.DefaultScheme =
        OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

var basketAccessPolicy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .RequireAssertion(context =>
        context.User.HasScope(
            BasketAuthorizationConstants.Scope))
    .Build();

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy(
        BasketAuthorizationConstants.AccessPolicy,
        basketAccessPolicy);

    authorizationOptions.FallbackPolicy = basketAccessPolicy;
});


// Add services to the container.

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
