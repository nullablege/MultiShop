using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Message.Authorization;
using MultiShop.Message.DTOs;
using MultiShop.Message.Services;
using OpenIddict.Abstractions;

namespace MultiShop.Message.Controllers;

[ApiController]
[Route("api/messages")]
[Authorize(Policy = MessageAuthorizationConstants.Policy)]
public sealed class MessagesController : ControllerBase
{
    private readonly IUserMessageService _userMessageService;

    public MessagesController(IUserMessageService userMessageService)
    {
        _userMessageService = userMessageService;
    }

    [HttpGet("inbox")]
    public async Task<ActionResult<IReadOnlyList<InboxMessageDto>>> GetInboxAsync(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var messages = await _userMessageService.GetInboxAsync(userId, cancellationToken);
        return Ok(messages);
    }

    [HttpGet("sent")]
    public async Task<ActionResult<IReadOnlyList<SentMessageDto>>> GetSentAsync(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var messages = await _userMessageService.GetSentAsync(userId, cancellationToken);
        return Ok(messages);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(
        CreateMessageDto createMessageDto,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var messageId = await _userMessageService.CreateAsync(
            userId,
            createMessageDto,
            cancellationToken);

        return Created($"api/messages/{messageId}", new { messageId });
    }

    [HttpPut("{messageId:int}/read")]
    public async Task<ActionResult> MarkAsReadAsync(
        int messageId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var updated = await _userMessageService.MarkAsReadAsync(
            messageId,
            userId,
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    private string? GetCurrentUserId()
    {
        var userId = User.FindFirst(OpenIddictConstants.Claims.Subject)?.Value;
        return string.IsNullOrWhiteSpace(userId) ? null : userId;
    }
}
