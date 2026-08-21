using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using MultiShop.WebUI.Authentication;
using MultiShop.WebUI.Configuration;
using MultiShop.WebUI.Handlers;
using MultiShop.WebUI.Services.Authentication;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CatalogServices.AboutServices;
using MultiShop.WebUI.Services.CatalogServices.BrandServices;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.ContactServices;
using MultiShop.WebUI.Services.CatalogServices.FeatureServices;
using MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices;
using MultiShop.WebUI.Services.CatalogServices.OfferDiscountServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.CatalogServices.SpecialOfferServices;
using MultiShop.WebUI.Services.CommentServices;
using MultiShop.WebUI.Services.DiscountServices;
using MultiShop.WebUI.Services.MessageServices;
using MultiShop.WebUI.Services.OrderServices;
using MultiShop.WebUI.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


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

builder.Services.AddOptions<CatalogClientCredentialsOptions>()
    .BindConfiguration(CatalogClientCredentialsOptions.SectionName)
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ClientId),
        "Catalog ziyaretçi ClientId zorunludur.")
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.ClientSecret),
        "Catalog ziyaretçi ClientSecret zorunludur.")
    .Validate(
        options => string.Equals(options.Scope, "catalog_api", StringComparison.Ordinal),
        "Catalog ziyaretçi kapsamı yalnızca catalog_api olmalıdır.")
    .ValidateOnStart();

builder.Services.AddOptions<ServiceApiOptions>()
    .BindConfiguration(ServiceApiOptions.SectionName)
    .Validate(options => Uri.TryCreate(options.GatewayBaseUrl, UriKind.Absolute, out var uri) && uri.Scheme == Uri.UriSchemeHttps, "Gateway adresi geçerli bir HTTPS adresi olmalıdır.")
    .Validate(options => new[] { options.Catalog, options.Discount, options.Order, options.Cargo, options.Basket, options.Comment, options.Message }.All(endpoint => !string.IsNullOrWhiteSpace(endpoint.Path) && !endpoint.Path.StartsWith('/') && endpoint.Path.EndsWith('/')), "Servis yolları dolu, göreli ve / ile biten adresler olmalıdır.")
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
        options.Cookie.Name = "__Host-MultiShop.WebUI.v2";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.SlidingExpiration = false;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
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
        options.UseTokenLifetime = false;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("offline_access");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("roles");
        options.Scope.Add("catalog_api");
        options.Scope.Add("basket_api");
        options.Scope.Add("comment_api");
        options.Scope.Add("discount_api");
        options.Scope.Add("identity_api");
        options.Scope.Add("order_api");
        options.Scope.Add("message_api");
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserAccessTokenService, UserAccessTokenService>();
builder.Services.AddTransient<UserAccessTokenHandler>();
builder.Services.AddTransient<CatalogClientCredentialsHandler>();
builder.Services.AddSingleton<ICatalogAccessTokenService, CatalogAccessTokenService>();
builder.Services.AddHttpClient("IdentityProvider", client =>
{
    client.BaseAddress = new Uri(identityProviderSettings.Authority.TrimEnd('/') + "/");
});

builder.Services
    .AddHttpClient<IUserService, UserService>(client =>
    {
        client.BaseAddress = new Uri(identityProviderSettings.Authority.TrimEnd('/') + "/");
    })
    .AddHttpMessageHandler<UserAccessTokenHandler>();

builder.Services
    .AddHttpClient<ICategoryService, CategoryService>((serviceProvider, client) =>
    {
        var options = serviceProvider.GetRequiredService<IOptions<ServiceApiOptions>>().Value;
        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Catalog.Path);
    })
    .AddHttpMessageHandler<CatalogClientCredentialsHandler>();

builder.Services
    .AddHttpClient<IProductService, ProductService>((serviceProvider, client) =>
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<ServiceApiOptions>>()
            .Value;

        client.BaseAddress = new Uri(options.GatewayBaseUrl + options.Catalog.Path);
    })
    .AddHttpMessageHandler<CatalogClientCredentialsHandler>();

builder.Services
    .AddHttpClient<IFeatureSliderService, FeatureSliderService>((serviceProvider, client) =>
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<ServiceApiOptions>>()
            .Value;

        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Catalog.Path);
    })
    .AddHttpMessageHandler<CatalogClientCredentialsHandler>();

builder.Services
    .AddHttpClient<ISpecialOfferService, SpecialOfferService>((serviceProvider, client) =>
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<ServiceApiOptions>>()
            .Value;

        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Catalog.Path);
    })
    .AddHttpMessageHandler<CatalogClientCredentialsHandler>();

builder.Services
    .AddHttpClient<IFeatureService, FeatureService>((serviceProvider, client) =>
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<ServiceApiOptions>>()
            .Value;

        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Catalog.Path);
    })
    .AddHttpMessageHandler<CatalogClientCredentialsHandler>();

builder.Services
    .AddHttpClient<IOfferDiscountService, OfferDiscountService>((serviceProvider, client) =>
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<ServiceApiOptions>>()
            .Value;

        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Catalog.Path);
    })
    .AddHttpMessageHandler<CatalogClientCredentialsHandler>();

builder.Services
    .AddHttpClient<IBrandService, BrandService>((serviceProvider, client) =>
    {
        var options = serviceProvider
            .GetRequiredService<IOptions<ServiceApiOptions>>()
            .Value;

        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Catalog.Path);
    })
    .AddHttpMessageHandler<CatalogClientCredentialsHandler>();

builder.Services
    .AddHttpClient<IAboutService, AboutService>((serviceProvider, client) =>
    {
        var options = serviceProvider.GetRequiredService<IOptions<ServiceApiOptions>>().Value;
        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Catalog.Path);
    })
    .AddHttpMessageHandler<CatalogClientCredentialsHandler>();

builder.Services
    .AddHttpClient<ICommentService, CommentService>((serviceProvider, client) =>
    {
        var options = serviceProvider.GetRequiredService<IOptions<ServiceApiOptions>>().Value;
        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Comment.Path);
    })
    .AddHttpMessageHandler<UserAccessTokenHandler>();

builder.Services
    .AddHttpClient<IPublicCommentService, PublicCommentService>((serviceProvider, client) =>
    {
        var options = serviceProvider.GetRequiredService<IOptions<ServiceApiOptions>>().Value;
        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Comment.Path);
    });

builder.Services
    .AddHttpClient<IBasketService, BasketService>((serviceProvider, client) =>
    {
        var options = serviceProvider.GetRequiredService<IOptions<ServiceApiOptions>>().Value;
        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Basket.Path);
    })
    .AddHttpMessageHandler<UserAccessTokenHandler>();

builder.Services
    .AddHttpClient<IDiscountService, DiscountService>((serviceProvider, client) =>
    {
        var options = serviceProvider.GetRequiredService<IOptions<ServiceApiOptions>>().Value;
        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Discount.Path);
    })
    .AddHttpMessageHandler<UserAccessTokenHandler>();

builder.Services
    .AddHttpClient<IOrderAddressService, OrderAddressService>((serviceProvider, client) =>
    {
        var options = serviceProvider.GetRequiredService<IOptions<ServiceApiOptions>>().Value;
        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Order.Path);
    })
    .AddHttpMessageHandler<UserAccessTokenHandler>();

builder.Services
    .AddHttpClient<IOrderHistoryService, OrderHistoryService>((serviceProvider, client) =>
    {
        var options = serviceProvider.GetRequiredService<IOptions<ServiceApiOptions>>().Value;
        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Order.Path);
    })
    .AddHttpMessageHandler<UserAccessTokenHandler>();

builder.Services
    .AddHttpClient<IMessageService, MessageService>((serviceProvider, client) =>
    {
        var options = serviceProvider.GetRequiredService<IOptions<ServiceApiOptions>>().Value;
        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Message.Path);
    })
    .AddHttpMessageHandler<UserAccessTokenHandler>();

builder.Services
    .AddHttpClient<IContactService, ContactService>((serviceProvider, client) =>
    {
        var options = serviceProvider.GetRequiredService<IOptions<ServiceApiOptions>>().Value;
        client.BaseAddress = new Uri(new Uri(options.GatewayBaseUrl), options.Catalog.Path);
    })
    .AddHttpMessageHandler<CatalogClientCredentialsHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<UserAuthenticationChallengeMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "user",
    areaName: "User",
    pattern: "User/{controller=Dashboard}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
