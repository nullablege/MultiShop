using MediatR;
using MultiShop.Order.Application.Features.CQRS.Queries.AddressQueries;
using MultiShop.Order.Application.Features.CQRS.Results.AddressResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, IReadOnlyList<GetAddressQueryResult>>
    {
        private readonly IRepository<Address> _repository;

        public GetAddressQueryHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<GetAddressQueryResult>> Handle(GetAddressQuery request, CancellationToken cancellationToken)
        {
            var addresses = await _repository.GetAllAsync(cancellationToken);

            return addresses.Select(address => new GetAddressQueryResult
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                District = address.District,
                City = address.City,
                Detail = address.Detail1
            }).ToList();
        }
    }
}
