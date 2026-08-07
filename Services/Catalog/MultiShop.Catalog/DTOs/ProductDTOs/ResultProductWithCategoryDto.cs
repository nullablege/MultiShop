namespace MultiShop.Catalog.DTOs.ProductDTOs
{
    public class ResultProductWithCategoryDto
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string CoverImageUrl { get; set; } = string.Empty;

        public string CategoryId { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
    }
}
