namespace MultiShop.WebUI.Models.Discount;

public sealed class DiscountCouponDto
{
    public int CouponId { get; set; }
    public string Code { get; set; } = string.Empty;
    public int Rate { get; set; }
    public bool IsActive { get; set; }
    public DateTime ValidDate { get; set; }
}
