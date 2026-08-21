
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using MultiShop.WebUI.Authentication;
using MultiShop.WebUI.Configuration;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace MultiShop.WebUI.Services.Authentication
{
    public sealed class UserAccessTokenService : IUserAccessTokenService
    {
        private static readonly TimeSpan RefreshSkew = TimeSpan.FromMinutes(1);

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IdentityProviderOptions _identityProviderOptions;

        public UserAccessTokenService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IOptions<IdentityProviderOptions> identityProviderOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _identityProviderOptions = identityProviderOptions.Value;
        }

        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HTTP context bulunamadı");

            var authenticationResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if(!authenticationResult.Succeeded || authenticationResult.Principal == null)
                throw new UserAuthenticationRequiredException("Kullanıcı oturumu bulunamadı.");

            var properties = authenticationResult.Properties ?? throw new UserAuthenticationRequiredException("Authentication özellikleri bulunamadı.");
            var accessToken = properties.GetTokenValue("access_token");
            var expireAtValue = properties.GetTokenValue("expires_at");

            if(string.IsNullOrWhiteSpace(accessToken))
                throw new UserAuthenticationRequiredException("Access token bulunamadı.");

            if (!DateTimeOffset.TryParse(expireAtValue, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var expiresAt))
                throw new UserAuthenticationRequiredException("Access token geçerlilik süresi okunamadı.");

            if (expiresAt > DateTimeOffset.UtcNow.Add(RefreshSkew))
                return accessToken;

            var refreshToken = properties.GetTokenValue("refresh_token");

            if(string.IsNullOrWhiteSpace(refreshToken))
                throw new UserAuthenticationRequiredException("Refresh token bulunamadı.");


            using var requestContent = new FormUrlEncodedContent([
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("client_id", _identityProviderOptions.ClientId),
                new KeyValuePair<string, string>("client_secret", _identityProviderOptions.ClientSecret )
                ]);

            using var response = await _httpClientFactory.CreateClient("IdentityProvider").PostAsync("connect/token", requestContent, cancellationToken);

            if (response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Unauthorized)
                throw new UserAuthenticationRequiredException("Refresh token geçersiz veya süresi dolmuş.");

            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>(cancellationToken: cancellationToken);

            if (tokenResponse == null || string.IsNullOrWhiteSpace(tokenResponse.AccessToken) || tokenResponse.ExpiresIn <= 0)
                throw new InvalidOperationException("Identity geçerli bir refresh-token yanıtı döndürmedi.");

            var newAccessToken = tokenResponse.AccessToken;
            var newRefreshToken = string.IsNullOrWhiteSpace(tokenResponse.RefreshToken) ? refreshToken : tokenResponse.RefreshToken;
            var newExpiresAt = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn);

            if (!properties.UpdateTokenValue("access_token", newAccessToken))
                throw new UserAuthenticationRequiredException("Cookie içindeki access token güncellenemedi.");

            if (!properties.UpdateTokenValue("refresh_token", newRefreshToken))
                throw new UserAuthenticationRequiredException("Cookie içindeki refresh token güncellenemedi.");

            if (!properties.UpdateTokenValue("expires_at", newExpiresAt.ToString("o", CultureInfo.InvariantCulture)))
                throw new UserAuthenticationRequiredException("Cookie içindeki token süresi güncellenemedi.");

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return newAccessToken;
        }
        private sealed class RefreshTokenResponse
        {
            [JsonPropertyName("access_token")]
            public string? AccessToken { get; init; }

            [JsonPropertyName("refresh_token")]
            public string? RefreshToken { get; init; }

            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; init; }
        }
    }
}
