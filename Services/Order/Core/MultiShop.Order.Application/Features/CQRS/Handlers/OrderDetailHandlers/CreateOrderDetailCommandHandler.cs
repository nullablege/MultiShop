using MediatR;
using MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class CreateOrderDetailCommandHandler : IRequestHandler<CreateOrderDetailCommand, int>
    {
        private readonly IRepository<OrderDetail> _repository;

        public CreateOrderDetailCommandHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateOrderDetailCommand request, CancellationToken cancellationToken)
        {
            var orderDetail = new OrderDetail
            {
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                ProductPrice = request.ProductPrice,
                ProductAmount = request.ProductAmount,
                ProductTotalPrice = request.ProductPrice * request.ProductAmount,
                OrderingId = request.OrderingId
            };

            var createdOrderDetail = await _repository.CreateAsync(orderDetail, cancellationToken);
            return createdOrderDetail.OrderDetailId;
        }
    }
}
