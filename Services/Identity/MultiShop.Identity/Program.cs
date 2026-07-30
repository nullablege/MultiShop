using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MultiShop.Identity.Configuration;
using MultiShop.Identity.Data;
using MultiShop.Identity.Models;
using MultiShop.Identity.Services;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("IdentityConnection")
    ?? throw new InvalidOperationException("Identity bağlantı metni bulunamadı");

builder.Services.AddDbContext<MultiShopIdentityDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseOpenIddict();
});

builder.Services
    .AddDefaultIdentity<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MultiShopIdentityDbContext>();

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
        .UseDbContext<MultiShopIdentityDbContext>();
    })
    .AddServer(options =>
    {
        options.SetAuthorizationEndpointUris("connect/authorize");
        options.SetTokenEndpointUris("connect/token");
        options.SetEndSessionEndpointUris("connect/logout");

        options.SetAuthorizationCodeLifetime(TimeSpan.FromMinutes(5));
        options.SetAccessTokenLifetime(TimeSpan.FromMinutes(15));

        options.AllowAuthorizationCodeFlow()
        .RequireProofKeyForCodeExchange();

        options.AllowClientCredentialsFlow();

        if (builder.Environment.IsDevelopment())
        {
            options.AddDevelopmentEncryptionCertificate()
                .AddDevelopmentSigningCertificate();
        }
        else
        {
            throw new InvalidOperationException(
                "Production OpenIddict sertifikaları yapılandırılmadı.");
        }

        options.UseAspNetCore()
                .EnableAuthorizationEndpointPassthrough()
                .EnableTokenEndpointPassthrough()
                .EnableEndSessionEndpointPassthrough();

        options.DisableAccessTokenEncryption();

        options.RegisterScopes(
                OpenIddictConstants.Scopes.Email,
                OpenIddictConstants.Scopes.Profile,
                OpenIddictConstants.Scopes.Roles,
                "catalog_api",
                "discount_api",
                "order_api");


    });

builder.Services
    .AddOptions<OpenIddictClientOptions>()
    .BindConfiguration(OpenIddictClientOptions.SectionName)
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ClientId),
        "OpenIddict WebUI ClientId bulunamadı.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ClientSecret),
        "OpenIddict WebUI ClientSecret bulunamadı.")
    .Validate(
        options => Uri.TryCreate(
            options.RedirectUri,
            UriKind.Absolute,
            out _),
        "OpenIddict WebUI RedirectUri geçerli değil.")
    .Validate(
        options => Uri.TryCreate(
            options.PostLogoutRedirectUri,
            UriKind.Absolute,
            out _),
        "OpenIddict WebUI PostLogoutRedirectUri geçerli değil.")
    .ValidateOnStart();

builder.Services
    .AddOptions<OpenIddictVisitorClientOptions>()
    .BindConfiguration(OpenIddictVisitorClientOptions.SectionName)
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ClientId),
        "OpenIddict Visitor ClientId bulunamadı.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ClientSecret),
        "OpenIddict Visitor ClientSecret bulunamadı.")
    .ValidateOnStart();

builder.Services.AddScoped<IOpenIddictPrincipalService, OpenIddictPrincipalService>();


builder.Services.AddHostedService<OpenIddictClientInitializer>();
builder.Services.AddHostedService<IdentityRoleInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
