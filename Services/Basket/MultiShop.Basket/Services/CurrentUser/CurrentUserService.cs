using OpenIddict.Abstractions;

namespace MultiShop.Basket.Services.CurrentUser
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId()
        {
            var value = _httpContextAccessor.HttpContext;
            if(value == null)
                throw new InvalidOperationException("HTTP isteği bulunamadı.");

            var userId = value.User.FindFirst(OpenIddictConstants.Claims.Subject)?.Value;

            if( string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("Token içerisinde kullanıcı ID bulunamadı");

            return userId;
        }
    }
}
