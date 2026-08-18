namespace MultiShop.OcelotGateway.Configuration
{
    public class IdentityProviderOptions
    {
        public const string SectionName = "IdentityProvider";
        public string Issuer { get; init; } = string.Empty;
    }
}
