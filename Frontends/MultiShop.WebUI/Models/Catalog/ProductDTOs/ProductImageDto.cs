namespace MultiShop.WebUI.Models.Catalog.ProductDTOs
{
    public class ProductImageDto
    {
        public string Url { get; set; } = string.Empty;
        public string AltText { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
