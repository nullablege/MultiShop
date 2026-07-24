namespace MultiShop.Catalog.Entities
{
    public class ProductImage
    {
        public string Url { get; set; } = string.Empty;
        public string AltText {  get; set; } = string.Empty;
        public int SortOrder {  get; set; }
    }
}
