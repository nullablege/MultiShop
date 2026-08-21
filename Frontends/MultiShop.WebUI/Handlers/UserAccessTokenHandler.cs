using MultiShop.WebUI.Services.Authentication;
using System.Net.Http.Headers;

namespace MultiShop.WebUI.Handlers
{
    public sealed class UserAccessTokenHandler : DelegatingHandler
    {
        private readonly IUserAccessTokenService _userAccessTokenService;

        public UserAccessTokenHandler(IUserAccessTokenService userAccessTokenService)
        {
            _userAccessTokenService = userAccessTokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _userAccessTokenService.GetAccessTokenAsync(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
