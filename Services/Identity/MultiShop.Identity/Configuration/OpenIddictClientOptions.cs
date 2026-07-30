namespace MultiShop.Identity.Configuration
{
    public sealed class OpenIddictClientOptions
    {
        public const string SectionName = "OpenIddict:WebClient";
        public string ClientId { get; init; } = string.Empty;
        public string RedirectUri { get; init; } = string.Empty;
        public string PostLogoutRedirectUri { get; init; } = string.Empty;
        public string ClientSecret { get; init; } = string.Empty;
    }
}
