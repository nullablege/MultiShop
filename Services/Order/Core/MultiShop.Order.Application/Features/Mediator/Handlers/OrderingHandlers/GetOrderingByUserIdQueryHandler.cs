using MediatR;
using MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class GetOrderingByUserIdQueryHandler : IRequestHandler<GetOrderingByUserIdQuery, IReadOnlyList<GetOrderingByUserIdQueryResult>>
    {
        private readonly IRepository<Ordering> _repository;

        public GetOrderingByUserIdQueryHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<GetOrderingByUserIdQueryResult>> Handle(GetOrderingByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orderings = await _repository.GetByFilterAsync(ordering => ordering.UserId == request.UserId, cancellationToken);

            return orderings.Select(ordering => new GetOrderingByUserIdQueryResult
            {
                OrderingId = ordering.OrderingId,
                UserId = ordering.UserId,
                TotalPrice = ordering.TotalPrice,
                OrderDate = ordering.OrderDate
            }).ToList();
        }
    }
}
