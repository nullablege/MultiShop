using MediatR;

namespace MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands
{
    public class CreateOrderDetailCommand : IRequest<int>
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int ProductAmount { get; set; }
        public int OrderingId { get; set; }
    }
}
