namespace MultiShop.WebUI.Models.Catalog.ProductDTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string CoverImageUrl { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public ProductDetailDto ProductDetail { get; set; } = new();
        public List<ProductImageDto> Images { get; set; } = new();
    }
}
