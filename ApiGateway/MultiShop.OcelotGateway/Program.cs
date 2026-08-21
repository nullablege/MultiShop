using MultiShop.OcelotGateway.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                     .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);


builder.Services.AddOptions<IdentityProviderOptions>()
    .BindConfiguration(IdentityProviderOptions.SectionName)
    .Validate(options =>
    {
        return Uri.TryCreate(options.Issuer, UriKind.Absolute, out var issueUri) && issueUri.Scheme == Uri.UriSchemeHttps;
    }, "IdentityProvider:Issuer ge�erli bir HTTPS adresi olmal�d�r. ")
    .ValidateOnStart();

var identityProviderOptions = builder.Configuration
    .GetRequiredSection(IdentityProviderOptions.SectionName)
    .Get<IdentityProviderOptions>() ?? throw new InvalidOperationException("IdentityProvider yap�land�rmas� bulunamad�");

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer(new Uri(identityProviderOptions.Issuer));

        options.AddAudiences(
            "catalog_api",
            "discount_api",
            "order_api",
            "cargo_api",
            "basket_api",
            "comment_api",
            "message_api"
            );

        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});



builder.Services.AddAuthorization();

builder.Services.AddOcelot(builder.Configuration);


var app = builder.Build();

app.UseHttpsRedirection();

await app.UseOcelot();
await app.RunAsync();
