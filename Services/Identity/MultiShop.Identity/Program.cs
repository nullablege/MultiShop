using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MultiShop.Identity.Authorization;
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

builder.Services
    .AddOptions<SmtpEmailOptions>()
    .BindConfiguration(SmtpEmailOptions.SectionName)
    .Validate(options => !string.IsNullOrWhiteSpace(options.Host), "SMTP sunucu adresi zorunludur.")
    .Validate(options => options.Port is > 0 and <= 65535, "SMTP portu 1-65535 aralığında olmalıdır.")
    .Validate(options => !string.IsNullOrWhiteSpace(options.SenderEmail), "Gönderen e-posta adresi zorunludur.")
    .Validate(options => options.TimeoutSeconds is > 0 and <= 120, "SMTP zaman aşımı 1-120 saniye aralığında olmalıdır.")
    .Validate(
        options => !options.UseAuthentication ||
                   (!string.IsNullOrWhiteSpace(options.UserName) && !string.IsNullOrWhiteSpace(options.Password)),
        "SMTP kimlik doğrulaması için kullanıcı adı ve parola zorunludur.")
    .ValidateOnStart();

builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

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
        options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

        options.AllowAuthorizationCodeFlow()
        .RequireProofKeyForCodeExchange();

        options.AllowRefreshTokenFlow();
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
                IdentityAuthorizationConstants.IdentityApiScope,
                "catalog_api",
                "discount_api",
                "order_api",
                "cargo_api",
                "basket_api",
                "comment_api",
                "message_api");


    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
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

if (builder.Environment.IsDevelopment())
{
    builder.Services
        .AddOptions<OpenIddictPostmanClientOptions>()
        .BindConfiguration(OpenIddictPostmanClientOptions.SectionName)
        .Validate(
            options => !string.IsNullOrWhiteSpace(options.ClientId),
            "OpenIddict Postman ClientId bulunamadı.")
        .Validate(
            options => Uri.TryCreate(
                options.RedirectUri,
                UriKind.Absolute,
                out _),
            "OpenIddict Postman RedirectUri geçerli değil.")
        .ValidateOnStart();
}

builder.Services.AddScoped<IOpenIddictPrincipalService, OpenIddictPrincipalService>();


builder.Services.AddHostedService<OpenIddictClientInitializer>();
builder.Services.AddHostedService<IdentityRoleInitializer>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        IdentityAuthorizationConstants.IdentityApiPolicy,
        policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireAssertion(context =>
                context.User.HasScope(
                    IdentityAuthorizationConstants.IdentityApiScope));
        });

    options.AddPolicy(
        IdentityAuthorizationConstants.ManagementPolicy,
        policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireAssertion(context =>
                context.User.HasScope(
                    IdentityAuthorizationConstants.IdentityApiScope));
            policy.RequireRole(
                IdentityAuthorizationConstants.AdminRole,
                IdentityAuthorizationConstants.ManagerRole);
        });
});

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
