namespace MultiShop.Order.Application.Features.CQRS.Results.OrderDetailResults
{
    public class GetOrderDetailByIdQueryResult
    {
        public int OrderDetailId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int ProductAmount { get; set; }
        public decimal ProductTotalPrice { get; set; }
        public int OrderingId { get; set; }
    }
}
