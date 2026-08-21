namespace MultiShop.WebUI.Models.Basket;

public sealed class CartViewModel
{
    private const decimal TaxRate = 0.10m;

    public BasketTotalDto Basket { get; init; } = new();
    public decimal Subtotal => Basket.TotalPrice;
    public decimal Tax => DiscountedSubtotal * TaxRate;
    public decimal TotalWithTax => DiscountedSubtotal + Tax;
    public string? CouponCode { get; init; }
    public int DiscountRate { get; init; }
    public bool HasDiscount => DiscountRate > 0;
    public decimal DiscountAmount => Subtotal * DiscountRate / 100;
    public decimal DiscountedSubtotal => Subtotal - DiscountAmount;
}
