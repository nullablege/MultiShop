namespace MultiShop.Identity.Configuration
{
    public sealed class OpenIddictVisitorClientOptions
    {
        public const string SectionName = "OpenIddict:VisitorClient";
        public string ClientId { get; init; } = string.Empty;
        public string ClientSecret { get; init; } = string.Empty;
    }
}
