namespace MultiShop.Catalog.Settings
{
    public class MongoDbSettings
    {
        public const string SectionName = "MongoDb";
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string CategoryCollectionName {  get; set; } = string.Empty;
        public string ProductCollectionName {  get; set; } = string.Empty;
        public string FeatureSliderCollectionName { get; set; } = string.Empty;
        public string SpecialOfferCollectionName { get; set; } = string.Empty;
        public string FeatureCollectionName { get; set; } = string.Empty;
        public string OfferDiscountCollectionName { get; set; } = string.Empty;
    }
}
