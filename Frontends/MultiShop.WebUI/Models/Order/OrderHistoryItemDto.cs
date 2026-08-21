namespace MultiShop.WebUI.Models.Order;

public sealed class OrderHistoryItemDto
{
    public int OrderingId { get; init; }
    public decimal TotalPrice { get; init; }
    public DateTime OrderDate { get; init; }
}
