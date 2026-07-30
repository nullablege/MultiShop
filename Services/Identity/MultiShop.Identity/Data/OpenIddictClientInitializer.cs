
using Microsoft.Extensions.Options;
using MultiShop.Identity.Configuration;
using OpenIddict.Abstractions;

namespace MultiShop.Identity.Data
{
    public sealed class OpenIddictClientInitializer : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly OpenIddictClientOptions _openIddictClientOptions;
        private readonly OpenIddictVisitorClientOptions _openIddictVisitorClientOptions;

        public OpenIddictClientInitializer(IServiceScopeFactory serviceScopeFactory, IOptions<OpenIddictClientOptions> openIddictClientOptions, IOptions<OpenIddictVisitorClientOptions> openIddictVisitorClientOptions)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _openIddictClientOptions = openIddictClientOptions.Value;
            _openIddictVisitorClientOptions = openIddictVisitorClientOptions.Value;
        }

        private async Task CreateVisitorClientAsync(IOpenIddictApplicationManager applicationManager, CancellationToken cancellationToken = default)
        {
            var application = await applicationManager.FindByClientIdAsync(_openIddictVisitorClientOptions.ClientId, cancellationToken);

            if (application != null)
            {
                return;
            }

            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientType = OpenIddictConstants.ClientTypes.Confidential,
                ClientId = _openIddictVisitorClientOptions.ClientId,
                ClientSecret = _openIddictVisitorClientOptions.ClientSecret,
                DisplayName = "MultiShop Visitor Client",

                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "catalog_api"
                }
            };

            await applicationManager.CreateAsync(descriptor, cancellationToken);

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var applicationManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            await CreateWebClientAsync(applicationManager, cancellationToken);
            await CreateVisitorClientAsync(applicationManager, cancellationToken);
        }

        private async Task CreateWebClientAsync(IOpenIddictApplicationManager applicationManager, CancellationToken cancellationToken)
        {
            var application = await applicationManager.FindByClientIdAsync(_openIddictClientOptions.ClientId, cancellationToken);
            if(application != null)
            {
                return;
            }
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ApplicationType = OpenIddictConstants.ApplicationTypes.Web,
                ClientType = OpenIddictConstants.ClientTypes.Confidential,
                ConsentType = OpenIddictConstants.ConsentTypes.Implicit,

                ClientId = _openIddictClientOptions.ClientId,
                ClientSecret = _openIddictClientOptions.ClientSecret,
                DisplayName = "MultiShop WebUI",

                RedirectUris =
                {
                    new Uri(_openIddictClientOptions.RedirectUri, UriKind.Absolute)
                },
                PostLogoutRedirectUris =
                {
                    new Uri(_openIddictClientOptions.PostLogoutRedirectUri, UriKind.Absolute)
                },

                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.Endpoints.Token,

                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,

                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,

                    OpenIddictConstants.Permissions.Prefixes.Scope + "catalog_api",
                    OpenIddictConstants.Permissions.Prefixes.Scope + "discount_api",
                    OpenIddictConstants.Permissions.Prefixes.Scope + "order_api"
                },

                Requirements =
                {
                    OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange
                }

            };

            await applicationManager.CreateAsync(descriptor, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
