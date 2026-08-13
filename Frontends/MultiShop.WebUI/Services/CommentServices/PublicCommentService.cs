using MultiShop.WebUI.Models.CommentDTOs;

namespace MultiShop.WebUI.Services.CommentServices
{
    public sealed class PublicCommentService : IPublicCommentService
    {
        private readonly HttpClient _httpClient;

        public PublicCommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<ResultCommentDto>> GetByProductIdAsync(
            string productId,
            CancellationToken cancellationToken = default)
        {
            var comments = await _httpClient.GetFromJsonAsync<List<ResultCommentDto>>(
                $"api/comments/by-product/{Uri.EscapeDataString(productId)}",
                cancellationToken);

            if (comments == null)
                return Array.Empty<ResultCommentDto>();

            return comments;
        }
    }
}
