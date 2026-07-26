using MediatR;
using MultiShop.Order.Application.Features.CQRS.Queries.OrderDetailQueries;
using MultiShop.Order.Application.Features.CQRS.Results.OrderDetailResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers
{
    public class GetOrderDetailQueryHandler : IRequestHandler<GetOrderDetailQuery, IReadOnlyList<GetOrderDetailQueryResult>>
    {
        private readonly IRepository<OrderDetail> _repository;

        public GetOrderDetailQueryHandler(IRepository<OrderDetail> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<GetOrderDetailQueryResult>> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            var orderDetails = await _repository.GetAllAsync(cancellationToken);

            return orderDetails.Select(orderDetail => new GetOrderDetailQueryResult
            {
                OrderDetailId = orderDetail.OrderDetailId,
                ProductId = orderDetail.ProductId,
                ProductName = orderDetail.ProductName,
                ProductPrice = orderDetail.ProductPrice,
                ProductAmount = orderDetail.ProductAmount,
                ProductTotalPrice = orderDetail.ProductTotalPrice,
                OrderingId = orderDetail.OrderingId
            }).ToList();
        }
    }
}
