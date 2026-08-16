using MultiShop.WebUI.Models.CommentDTOs;

namespace MultiShop.WebUI.Services.CommentServices
{
    public interface IPublicCommentService
    {
        Task<IReadOnlyList<ResultCommentDto>> GetByProductIdAsync(
            string productId,
            CancellationToken cancellationToken = default);
        Task CreateCommentAsync(
            CreateCommentDto createCommentDto,
            CancellationToken cancellationToken = default);
    }
}
