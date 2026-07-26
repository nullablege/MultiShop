using MediatR;

namespace MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands
{
    public class RemoveOrderingCommand : IRequest<bool>
    {
        public int OrderingId { get; }

        public RemoveOrderingCommand(int orderingId)
        {
            OrderingId = orderingId;
        }
    }
}
