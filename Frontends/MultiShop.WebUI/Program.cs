using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MultiShop.WebUI.Configuration;
using MultiShop.WebUI.Handlers;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddOptions<CatalogApiOptions>()
    .BindConfiguration(CatalogApiOptions.SectionName)
    .Validate(
        options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _),
        "Catalog API adresi geçerli bir mutlak URI olmalıdır.")
    .ValidateOnStart();

builder.Services.AddOptions<IdentityProviderOptions>()
    .BindConfiguration(IdentityProviderOptions.SectionName)
    .Validate(
        options => Uri.TryCreate(options.Authority, UriKind.Absolute, out _),
        "Identity Provider adresi geçerli bir mutlak URI olmalıdır.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ClientId),
        "Identity Provider ClientId zorunludur.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ClientSecret),
        "Identity Provider ClientSecret zorunludur.")
    .ValidateOnStart();

var identityProviderSettings = builder.Configuration
    .GetRequiredSection(IdentityProviderOptions.SectionName)
    .Get<IdentityProviderOptions>()
    ?? throw new InvalidOperationException(
        "Identity Provider yapılandırması bulunamadı.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme =
            CookieAuthenticationDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            OpenIdConnectDefaults.AuthenticationScheme;

        options.DefaultSignOutScheme =
            OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.Name = "__Host-MultiShop.WebUI";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.SlidingExpiration = false;
    })
    .AddOpenIdConnect(options =>
    {
        options.Authority = identityProviderSettings.Authority;
        options.ClientId = identityProviderSettings.ClientId;
        options.ClientSecret = identityProviderSettings.ClientSecret;

        options.ResponseType = OpenIdConnectResponseType.Code;
        options.UsePkce = true;
        options.SaveTokens = true;
        options.RequireHttpsMetadata = true;
        options.UseTokenLifetime = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("roles");
        options.Scope.Add("catalog_api");
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<UserAccessTokenHandler>();

builder.Services
    .AddHttpClient<ICategoryService, CategoryService>((serviceProvider, client) =>
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<CatalogApiOptions>>()
            .Value;

        client.BaseAddress = new Uri(options.BaseUrl);
    })
    .AddHttpMessageHandler<UserAccessTokenHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
