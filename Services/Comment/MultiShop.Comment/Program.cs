using Microsoft.EntityFrameworkCore;
using MultiShop.Comment.Authorization;
using MultiShop.Comment.Context;
using MultiShop.Comment.Settings;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("CommentDatabase")
    ?? throw new InvalidOperationException("CommentDatabase connection string bulunamadı.");

builder.Services.AddDbContext<CommentContext>(options =>
{
    options.UseSqlServer(connectionString);
});

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

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer(identityProviderIssuer);
        options.AddAudiences(CommentAuthorizationConstants.Audience);
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
        CommentAuthorizationConstants.Policy,
        policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireAssertion(context =>
                context.User.HasScope(CommentAuthorizationConstants.Scope));
        });

    options.AddPolicy(
        CommentAuthorizationConstants.ManagementPolicy,
        policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireAssertion(context =>
                context.User.HasScope(CommentAuthorizationConstants.Scope));
            policy.RequireRole(
                CommentAuthorizationConstants.AdminRole,
                CommentAuthorizationConstants.ManagerRole);
        });
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
