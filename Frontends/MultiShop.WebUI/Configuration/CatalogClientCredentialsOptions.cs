namespace MultiShop.WebUI.Configuration;

public sealed class CatalogClientCredentialsOptions
{
    public const string SectionName = "CatalogClientCredentials";

    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    public string Scope { get; init; } = "catalog_api";
}
