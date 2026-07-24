using MultiShop.Catalog.DTOs.ProductDetailDTOs;
using MultiShop.Catalog.DTOs.ProductImageDTOs;

namespace MultiShop.Catalog.DTOs.ProductDTOs
{
    public class UpdateProductDto
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string CoverImageUrl { get; set; } = string.Empty;

        public string CategoryId { get; set; } = string.Empty;
        public ProductDetailDto ProductDetail { get; set; } = new();
        public List<ProductImageDto> Images { get; set; } = new();
    }
}
