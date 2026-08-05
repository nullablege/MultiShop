using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace MultiShop.WebUI.Handlers
{
    public sealed class UserAccessTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                throw new InvalidOperationException("HTTP context bulunamadı.");

            var accessToken = await httpContext.GetTokenAsync("access_token");
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new InvalidOperationException("Access token bulunamadı.");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
