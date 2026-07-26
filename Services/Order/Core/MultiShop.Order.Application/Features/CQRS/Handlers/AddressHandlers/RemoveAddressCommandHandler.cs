using MediatR;
using MultiShop.Order.Application.Features.CQRS.Commands.AddressCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class RemoveAddressCommandHandler : IRequestHandler<RemoveAddressCommand, bool>
    {
        private readonly IRepository<Address> _repository;

        public RemoveAddressCommandHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(RemoveAddressCommand request, CancellationToken cancellationToken)
        {
            var address = await _repository.GetByIdAsync(request.AddressId, cancellationToken);
            if (address is null)
            {
                return false;
            }

            return await _repository.DeleteAsync(address, cancellationToken);
        }
    }
}
