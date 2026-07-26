using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Order.Application.Features.CQRS.Commands.OrderDetailCommands;
using MultiShop.Order.Application.Features.CQRS.Queries.OrderDetailQueries;
using MultiShop.Order.Application.Features.CQRS.Results.OrderDetailResults;

namespace MultiShop.Order.WebApi.Controllers
{
    [Route("api/order-details")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly ISender _sender;

        public OrderDetailsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetOrderDetailQueryResult>>> GetAll(CancellationToken cancellationToken)
        {
            var orderDetails = await _sender.Send(new GetOrderDetailQuery(), cancellationToken);
            return Ok(orderDetails);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetOrderDetailByIdQueryResult>> GetById(int id, CancellationToken cancellationToken)
        {
            var orderDetail = await _sender.Send(new GetOrderDetailByIdQuery(id), cancellationToken);
            if (orderDetail is null)
            {
                return NotFound();
            }

            return Ok(orderDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDetailCommand command, CancellationToken cancellationToken)
        {
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateOrderDetailCommand command, CancellationToken cancellationToken)
        {
            if (id != command.OrderDetailId)
            {
                return BadRequest();
            }

            var updated = await _sender.Send(command, cancellationToken);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _sender.Send(new RemoveOrderDetailCommand(id), cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
