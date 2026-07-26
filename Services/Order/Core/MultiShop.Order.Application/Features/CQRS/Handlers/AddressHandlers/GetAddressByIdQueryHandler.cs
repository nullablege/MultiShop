using MediatR;
using MultiShop.Order.Application.Features.CQRS.Queries.AddressQueries;
using MultiShop.Order.Application.Features.CQRS.Results.AddressResults;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, GetAddressByIdResult?>
    {
        private readonly IRepository<Address> _repository;

        public GetAddressByIdQueryHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task<GetAddressByIdResult?> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var result =  await _repository.GetByIdAsync(request.AddressId, cancellationToken);
            if (result == null)
                return null;

            return new GetAddressByIdResult
            {
                AddressId = result.AddressId,
                UserId = result.UserId,
                Name = result.Name,
                Surname = result.Surname,
                Email = result.Email,
                Phone = result.Phone,
                Country = result.Country,
                District = result.District,
                City = result.City,
                Detail1 = result.Detail1,
                Detail2 = result.Detail2,
                Description = result.Description,
                ZipCode = result.ZipCode
            };
        }
    }
}
