using MediatR;
using MultiShop.Order.Application.Features.CQRS.Commands.AddressCommands;
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
    public class CreateAddressCommandHandler:IRequestHandler<CreateAddressCommand,int>
    {
        private readonly IRepository<Address> _repository;
        public CreateAddressCommandHandler(IRepository<Address> repository) {
            _repository = repository;
        }

        public async Task<int> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var element = new Address
            {
                UserId = request.UserId,
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                Phone = request.Phone,
                Country = request.Country,
                District = request.District,
                City = request.City,
                Detail1 = request.Detail1,
                Detail2 = request.Detail2,
                Description = request.Description,
                ZipCode = request.ZipCode
            };

            var created = await _repository.CreateAsync(element, cancellationToken);
            return created.AddressId;
        }
    }
}
