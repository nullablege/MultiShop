using MultiShop.Discount.Authorization;
using MultiShop.Discount.Configuration;
using MultiShop.Discount.Data;
using MultiShop.Discount.Services;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddOptions<SqlServerOptions>()
    .Bind(builder.Configuration.GetSection(SqlServerOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.ConnectionString), "SqlServer:ConnectionString yapılandırılmalıdır.")
    .ValidateOnStart();

builder.Services
    .AddOptions<IdentityProviderOptions>()
    .Bind(builder.Configuration.GetSection(IdentityProviderOptions.SectionName))
    .Validate(options =>
    {
        if (!Uri.TryCreate(options.Issuer, UriKind.Absolute, out var issuerUri))
        {
            return false;
        }

        return issuerUri.Scheme == Uri.UriSchemeHttp ||
               issuerUri.Scheme == Uri.UriSchemeHttps;
    }, "IdentityProvider:Issuer geçerli bir HTTP veya HTTPS adresi olmalıdır.")
    .ValidateOnStart();

var identityProviderOptions = builder.Configuration
    .GetRequiredSection(IdentityProviderOptions.SectionName)
    .Get<IdentityProviderOptions>()
    ?? throw new InvalidOperationException(
        "IdentityProvider yapılandırması bulunamadı.");

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer(new Uri(identityProviderOptions.Issuer));
        options.AddAudiences(DiscountAuthorizationConstants.Audience);
        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme =
        OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        DiscountAuthorizationConstants.Policy,
        policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireAssertion(context =>
                context.User.HasScope(
                    DiscountAuthorizationConstants.Scope));
        });
});

builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
