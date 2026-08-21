namespace MultiShop.WebUI.Models.Basket;

public sealed class BasketTotalDto
{
    public string UserId { get; set; } = string.Empty;
    public string? DiscountCode { get; set; }
    public int DiscountRate { get; set; }
    public List<BasketItemDto> BasketItems { get; set; } = new();
    public decimal TotalPrice => BasketItems.Sum(item => item.LineTotal);
}
