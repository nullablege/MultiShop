namespace MultiShop.Basket.Dtos
{
    public class BasketTotalDto
    {
        public string UserId { get; set; } = string.Empty;
        public string? DiscountCode { get; set; }
        public int DiscountRate { get; set; }
        public List<BasketItemDto> BasketItems { get; set; } = new List<BasketItemDto>();
        public decimal TotalPrice => BasketItems.Sum(x => x.Price * x.Quantity);
    }
}
