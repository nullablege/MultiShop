namespace MultiShop.Cargo.WebApi.Configuration
{
    public sealed class IdentityProviderOptions
    {
        public const string SectionName = "IdentityProvider";
        public string Issuer { get; init; } = string.Empty;
    }
}
