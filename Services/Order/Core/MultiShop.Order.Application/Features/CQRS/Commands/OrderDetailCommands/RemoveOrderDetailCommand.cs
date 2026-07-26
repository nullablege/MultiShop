using MediatR;

namespace MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands
{
    public class RemoveOrderDetailCommand : IRequest<bool>
    {
        public int OrderDetailId { get; }

        public RemoveOrderDetailCommand(int orderDetailId)
        {
            OrderDetailId = orderDetailId;
        }
    }
}
