using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CommentServices;

namespace MultiShop.WebUI.ViewComponents;

public sealed class ProductReviewViewComponent : ViewComponent
{
    private readonly IPublicCommentService _publicCommentService;

    public ProductReviewViewComponent(IPublicCommentService publicCommentService)
    {
        _publicCommentService = publicCommentService;
    }

    public async Task<IViewComponentResult> InvokeAsync(
        string productId,
        CancellationToken cancellationToken)
    {
        var comments = await _publicCommentService.GetByProductIdAsync(
            productId,
            cancellationToken);

        return View(comments);
    }
}
