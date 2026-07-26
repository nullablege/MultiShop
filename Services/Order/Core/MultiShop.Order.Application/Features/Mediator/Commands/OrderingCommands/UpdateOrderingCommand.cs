using MediatR;

namespace MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands
{
    public class UpdateOrderingCommand : IRequest<bool>
    {
        public int OrderingId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
