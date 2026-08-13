using MultiShop.WebUI.Models.CommentDTOs;

namespace MultiShop.WebUI.Services.CommentServices
{
    public interface ICommentService
    {
        Task<IReadOnlyList<AdminCommentListDto>> GetAdminCommentsAsync(
            CancellationToken cancellationToken = default);
        Task<AdminCommentListDto?> GetAdminCommentAsync(
            int commentId,
            CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(
            int commentId,
            UpdateCommentStatusDto updateCommentStatusDto,
            CancellationToken cancellationToken = default);
        Task DeleteAsync(
            int commentId,
            CancellationToken cancellationToken = default);
    }
}
