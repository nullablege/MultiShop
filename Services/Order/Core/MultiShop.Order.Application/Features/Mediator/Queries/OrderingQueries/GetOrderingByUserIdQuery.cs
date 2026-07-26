using MediatR;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;

namespace MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries
{
    public class GetOrderingByUserIdQuery : IRequest<IReadOnlyList<GetOrderingByUserIdQueryResult>>
    {
        public string UserId { get; }

        public GetOrderingByUserIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
