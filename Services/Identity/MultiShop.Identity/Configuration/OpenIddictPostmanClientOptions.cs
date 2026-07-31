namespace MultiShop.Identity.Configuration
{
    public sealed class OpenIddictPostmanClientOptions
    {
        public const string SectionName = "OpenIddict:PostmanClient";

        public string ClientId { get; init; } = string.Empty;
        public string RedirectUri { get; init; } = string.Empty;
    }
}
