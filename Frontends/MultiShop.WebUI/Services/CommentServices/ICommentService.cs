using MultiShop.WebUI.Models.CommentDTOs;

namespace MultiShop.WebUI.Services.CommentServices
{
    public interface ICommentService
    {
        Task<IReadOnlyList<AdminCommentListDto>> GetAdminCommentsAsync(
            CancellationToken cancellationToken = default);
    }
}
