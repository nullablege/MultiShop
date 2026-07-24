namespace MultiShop.Catalog.Settings
{
    public class MongoDbSettings
    {
        public const string SectionName = "MongoDb";
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string CategoryCollectionName {  get; set; } = string.Empty;
        public string ProductCollectionName {  get; set; } = string.Empty;
    }
}
