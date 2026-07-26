using MediatR;

namespace MultiShop.Order.Application.Features.CQRS.Commands.AddressCommands
{
    public class RemoveAddressCommand : IRequest<bool>
    {
        public int AddressId { get; }

        public RemoveAddressCommand(int addressId)
        {
            AddressId = addressId;
        }
    }
}
