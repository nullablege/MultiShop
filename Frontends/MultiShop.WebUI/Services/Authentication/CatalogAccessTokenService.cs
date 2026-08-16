using Microsoft.Extensions.Options;
using MultiShop.WebUI.Configuration;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MultiShop.WebUI.Services.Authentication;

public sealed class CatalogAccessTokenService : ICatalogAccessTokenService
{
    private static readonly TimeSpan RefreshSkew = TimeSpan.FromMinutes(1);

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly CatalogClientCredentialsOptions _options;
    private readonly SemaphoreSlim _tokenLock = new(1, 1);
    private string? _accessToken;
    private DateTimeOffset _expiresAt = DateTimeOffset.MinValue;

    public CatalogAccessTokenService(
        IHttpClientFactory httpClientFactory,
        IOptions<CatalogClientCredentialsOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (HasUsableToken())
            return _accessToken!;

        await _tokenLock.WaitAsync(cancellationToken);

        try
        {
            if (HasUsableToken())
                return _accessToken!;

            using var requestContent = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _options.ClientId),
                new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
                new KeyValuePair<string, string>("scope", _options.Scope)
            ]);

            using var response = await _httpClientFactory
                .CreateClient("IdentityProvider")
                .PostAsync("connect/token", requestContent, cancellationToken);

            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content
                .ReadFromJsonAsync<TokenResponse>(cancellationToken: cancellationToken)
                ?? throw new InvalidOperationException("Identity token yanıtı alınamadı.");

            if (string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
                throw new InvalidOperationException("Identity token yanıtı access token içermiyor.");

            _accessToken = tokenResponse.AccessToken;
            _expiresAt = DateTimeOffset.UtcNow.AddSeconds(
                Math.Max(1, tokenResponse.ExpiresIn - (int)RefreshSkew.TotalSeconds));

            return _accessToken;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    private bool HasUsableToken() =>
        !string.IsNullOrWhiteSpace(_accessToken) && DateTimeOffset.UtcNow < _expiresAt;

    private sealed class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; init; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }
    }
}
