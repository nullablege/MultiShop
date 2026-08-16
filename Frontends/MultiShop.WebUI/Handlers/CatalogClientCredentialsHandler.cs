using MultiShop.WebUI.Services.Authentication;
using System.Net.Http.Headers;

namespace MultiShop.WebUI.Handlers;

public sealed class CatalogClientCredentialsHandler : DelegatingHandler
{
    private readonly ICatalogAccessTokenService _catalogAccessTokenService;

    public CatalogClientCredentialsHandler(
        ICatalogAccessTokenService catalogAccessTokenService)
    {
        _catalogAccessTokenService = catalogAccessTokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var accessToken = await _catalogAccessTokenService.GetAccessTokenAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
