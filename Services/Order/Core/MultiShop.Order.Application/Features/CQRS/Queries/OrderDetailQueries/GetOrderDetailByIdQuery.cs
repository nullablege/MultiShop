using MediatR;
using MultiShop.Order.Application.Features.CQRS.Results.OrderDetailResults;

namespace MultiShop.Order.Application.Features.CQRS.Queries.OrderDetailQueries
{
    public class GetOrderDetailByIdQuery : IRequest<GetOrderDetailByIdQueryResult?>
    {
        public int OrderDetailId { get; }

        public GetOrderDetailByIdQuery(int orderDetailId)
        {
            OrderDetailId = orderDetailId;
        }
    }
}
