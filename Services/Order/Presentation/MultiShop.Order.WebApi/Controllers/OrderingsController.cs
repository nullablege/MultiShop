using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Order.Application.Features.Mediator.Commands.OrderingCommands;
using MultiShop.Order.Application.Features.Mediator.Queries.OrderingQueries;
using MultiShop.Order.Application.Features.Mediator.Results.OrderingResults;
using MultiShop.Order.WebApi.Authorization;
using OpenIddict.Abstractions;
using System.Security.Claims;

namespace MultiShop.Order.WebApi.Controllers
{
    [Route("api/orderings")]
    [ApiController]
    [Authorize(Policy = OrderAuthorizationConstants.AccessPolicy)]
    public class OrderingsController : ControllerBase
    {
        private readonly ISender _sender;
        public OrderingsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize(Policy = OrderAuthorizationConstants.ManagementPolicy)]
        public async Task<ActionResult<IReadOnlyList<GetOrderingQueryResult>>> GetAll(CancellationToken cancellationToken)
        {
            var orderings = await _sender.Send(new GetOrderingQuery(), cancellationToken);
            return Ok(orderings);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = OrderAuthorizationConstants.ManagementPolicy)]
        public async Task<ActionResult<GetOrderingByIdQueryResult>> GetById(int id, CancellationToken cancellationToken)
        {
            var ordering = await _sender.Send(new GetOrderingByIdQuery(id), cancellationToken);
            if (ordering is null)
            {
                return NotFound();
            }

            return Ok(ordering);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Policy = OrderAuthorizationConstants.ManagementPolicy)]
        public async Task<ActionResult<IReadOnlyList<GetOrderingByUserIdQueryResult>>> GetByUserId(string userId, CancellationToken cancellationToken)
        {
            var orderings = await _sender.Send(new GetOrderingByUserIdQuery(userId), cancellationToken);
            return Ok(orderings);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderingCommand command, CancellationToken cancellationToken)
        {
            await _sender.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = OrderAuthorizationConstants.ManagementPolicy)]
        public async Task<IActionResult> Update(int id, UpdateOrderingCommand command, CancellationToken cancellationToken)
        {
            if (id != command.OrderingId)
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

        [HttpGet("me")]
        public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(OpenIddictConstants.Claims.Subject);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var orderings = await _sender.Send(new GetOrderingByUserIdQuery(userId), cancellationToken);
            return Ok(orderings);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = OrderAuthorizationConstants.ManagementPolicy)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var deleted = await _sender.Send(new RemoveOrderingCommand(id), cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
