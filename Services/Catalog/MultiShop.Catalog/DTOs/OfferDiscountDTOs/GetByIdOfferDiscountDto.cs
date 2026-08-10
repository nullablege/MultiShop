namespace MultiShop.Catalog.DTOs.OfferDiscountDTOs
{
    public class GetByIdOfferDiscountDto
    {
        public string OfferDiscountId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string ButtonTitle { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
