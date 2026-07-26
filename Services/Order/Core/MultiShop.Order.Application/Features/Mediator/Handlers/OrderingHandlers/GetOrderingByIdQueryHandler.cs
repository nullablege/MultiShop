using MediatR;
using MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class GetOrderingByIdQueryHandler : IRequestHandler<GetOrderingByIdQuery, GetOrderingByIdQueryResult?>
    {
        private readonly IRepository<Ordering> _repository;

        public GetOrderingByIdQueryHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task<GetOrderingByIdQueryResult?> Handle(GetOrderingByIdQuery request, CancellationToken cancellationToken)
        {
            var ordering = await _repository.GetByIdAsync(request.OrderingId, cancellationToken);
            if (ordering is null)
            {
                return null;
            }

            return new GetOrderingByIdQueryResult
            {
                OrderingId = ordering.OrderingId,
                UserId = ordering.UserId,
                TotalPrice = ordering.TotalPrice,
                OrderDate = ordering.OrderDate
            };
        }
    }
}
