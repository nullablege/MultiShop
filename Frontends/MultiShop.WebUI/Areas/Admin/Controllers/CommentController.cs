using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}
