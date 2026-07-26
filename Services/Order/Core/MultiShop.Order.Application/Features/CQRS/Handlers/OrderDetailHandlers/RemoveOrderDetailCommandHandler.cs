using MediatR;
using MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class RemoveOrderDetailCommandHandler : IRequestHandler<RemoveOrderDetailCommand, bool>
    {
        private readonly IRepository<OrderDetail> _repository;

        public RemoveOrderDetailCommandHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(RemoveOrderDetailCommand request, CancellationToken cancellationToken)
        {
            var orderDetail = await _repository.GetByIdAsync(request.OrderDetailId, cancellationToken);
            if (orderDetail is null)
            {
                return false;
            }

            return await _repository.DeleteAsync(orderDetail, cancellationToken);
        }
    }
}
