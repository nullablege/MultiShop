using MediatR;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;

namespace MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries
{
    public class GetOrderingByIdQuery : IRequest<GetOrderingByIdQueryResult?>
    {
        public int OrderingId { get; }

        public GetOrderingByIdQuery(int orderingId)
        {
            OrderingId = orderingId;
        }
    }
}
