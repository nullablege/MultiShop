namespace MultiShop.WebUI.Configuration
{
    public sealed class IdentityProviderOptions
    {
        public const string SectionName = "IdentityProvider";
        public string Authority { get; init; } = string.Empty;
        public string ClientId { get; init; } = string.Empty;
        public string ClientSecret { get; init; } = string.Empty;
    }
}
