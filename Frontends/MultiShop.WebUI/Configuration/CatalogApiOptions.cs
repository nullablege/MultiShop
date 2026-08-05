namespace MultiShop.WebUI.Configuration
{
    public sealed class CatalogApiOptions
    {
        public const string SectionName = "CatalogApi";
        public string BaseUrl { get; init; } = string.Empty;
    }
}
