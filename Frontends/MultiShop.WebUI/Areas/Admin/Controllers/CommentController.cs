using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models.CommentDTOs;
using MultiShop.WebUI.Services.CommentServices;

namespace MultiShop.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var model = await _commentService.GetAdminCommentsAsync(cancellationToken);
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> UpdateComment(
        int id,
        CancellationToken cancellationToken)
    {
        var model = await _commentService.GetAdminCommentAsync(id, cancellationToken);

        if (model == null)
            return NotFound();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateComment(
        int userCommentId,
        bool status,
        CancellationToken cancellationToken)
    {
        await _commentService.UpdateStatusAsync(
            userCommentId,
            new UpdateCommentStatusDto
            {
                Status = status
            },
            cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteComment(
        int userCommentId,
        CancellationToken cancellationToken)
    {
        await _commentService.DeleteAsync(userCommentId, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
