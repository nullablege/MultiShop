namespace MultiShop.WebUI.Configuration
{
    public sealed class ServiceApiOptions
    {
        public const string SectionName = "ServiceApi";

        public string GatewayBaseUrl { get; init; } = string.Empty;
        public ServiceEndpointOptions Catalog { get; init; } = new();
        public ServiceEndpointOptions Discount { get; init; } = new();
        public ServiceEndpointOptions Order { get; init; } = new();
        public ServiceEndpointOptions Cargo { get; init; } = new();
        public ServiceEndpointOptions Basket { get; init; } = new();
        public ServiceEndpointOptions Comment { get; init; } = new();
        public ServiceEndpointOptions Message { get; init; } = new();

    }
    public sealed class ServiceEndpointOptions
    {
        public string Path { get; init; } = string.Empty;
    }
}
