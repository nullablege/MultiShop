using MediatR;
using MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class GetOrderingQueryHandler : IRequestHandler<GetOrderingQuery, IReadOnlyList<GetOrderingQueryResult>>
    {
        private readonly IRepository<Ordering> _repository;

        public GetOrderingQueryHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<GetOrderingQueryResult>> Handle(GetOrderingQuery request, CancellationToken cancellationToken)
        {
            var orderings = await _repository.GetAllAsync(cancellationToken);

            return orderings.Select(ordering => new GetOrderingQueryResult
            {
                OrderingId = ordering.OrderingId,
                UserId = ordering.UserId,
                TotalPrice = ordering.TotalPrice,
                OrderDate = ordering.OrderDate
            }).ToList();
        }
    }
}
