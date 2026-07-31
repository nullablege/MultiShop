using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Order.Application.Features.CQRS.Commands.AddressCommands;
using MultiShop.Order.Application.Features.CQRS.Queries.AddressQueries;
using MultiShop.Order.Application.Features.CQRS.Results.AddressResults;
using MultiShop.Order.WebApi.Authorization;

namespace MultiShop.Order.WebApi.Controllers
{
    [Route("api/addresses")]
    [ApiController]
    [Authorize(Policy = OrderAuthorizationConstants.ManagementPolicy)]
    public class AddressesController : ControllerBase
    {
        private readonly ISender _sender;

        public AddressesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<GetAddressQueryResult>>> GetAll(CancellationToken cancellationToken)
        {
            var addresses = await _sender.Send(new GetAddressQuery(), cancellationToken);
            return Ok(addresses);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetAddressByIdResult>> GetById(int id, CancellationToken cancellationToken)
        {
            var address = await _sender.Send(new GetAddressByIdQuery(id), cancellationToken);
            if (address is null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAddressCommand command, CancellationToken cancellationToken)
        {
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateAddressCommand command, CancellationToken cancellationToken)
        {
            if (id != command.AddressId)
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
            var deleted = await _sender.Send(new RemoveAddressCommand(id), cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
