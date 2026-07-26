using MediatR;

namespace MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands
{
    public class CreateOrderingCommand : IRequest<int>
    {
        public string UserId { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
