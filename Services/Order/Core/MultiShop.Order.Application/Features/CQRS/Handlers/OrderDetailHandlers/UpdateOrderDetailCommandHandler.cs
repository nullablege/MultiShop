using MediatR;
using MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class UpdateOrderDetailCommandHandler : IRequestHandler<UpdateOrderDetailCommand, bool>
    {
        private readonly IRepository<OrderDetail> _repository;

        public UpdateOrderDetailCommandHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
        {
            var orderDetail = await _repository.GetByIdAsync(request.OrderDetailId, cancellationToken);
            if (orderDetail is null)
            {
                return false;
            }

            orderDetail.ProductId = request.ProductId;
            orderDetail.ProductName = request.ProductName;
            orderDetail.ProductPrice = request.ProductPrice;
            orderDetail.ProductAmount = request.ProductAmount;
            orderDetail.ProductTotalPrice = request.ProductPrice * request.ProductAmount;
            orderDetail.OrderingId = request.OrderingId;

            return await _repository.UpdateAsync(orderDetail, cancellationToken);
        }
    }
}
