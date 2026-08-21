using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.MessageServices;

namespace MultiShop.WebUI.Areas.User.Controllers;

[Area("User")]
[Authorize]
public sealed class MessageController : Controller
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<IActionResult> Inbox(CancellationToken cancellationToken)
    {
        var messages = await _messageService.GetInboxAsync(cancellationToken);
        return View(messages);
    }

    [HttpGet]
    public async Task<IActionResult> Sent(CancellationToken cancellationToken)
    {
        var messages = await _messageService.GetSentAsync(cancellationToken);
        return View(messages);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(
        int messageId,
        CancellationToken cancellationToken)
    {
        var updated = await _messageService.MarkAsReadAsync(
            messageId,
            cancellationToken);

        if (!updated)
        {
            TempData["MessageError"] = "Mesaj bulunamadı veya bu kullanıcıya ait değil.";
        }

        return RedirectToAction(nameof(Inbox));
    }
}
