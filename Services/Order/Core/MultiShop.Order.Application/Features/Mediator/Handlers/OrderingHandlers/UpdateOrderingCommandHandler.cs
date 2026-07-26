using MediatR;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Domain.Entities;

namespace MultiShop.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class UpdateOrderingCommandHandler : IRequestHandler<UpdateOrderingCommand, bool>
    {
        private readonly IRepository<Ordering> _repository;

        public UpdateOrderingCommandHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(UpdateOrderingCommand request, CancellationToken cancellationToken)
        {
            var ordering = await _repository.GetByIdAsync(request.OrderingId, cancellationToken);
            if (ordering is null)
            {
                return false;
            }

            ordering.UserId = request.UserId;
            ordering.TotalPrice = request.TotalPrice;
            ordering.OrderDate = request.OrderDate;

            return await _repository.UpdateAsync(ordering, cancellationToken);
        }
    }
}
