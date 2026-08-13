using MultiShop.WebUI.Models.CommentDTOs;

namespace MultiShop.WebUI.Services.CommentServices
{
    public sealed class CommentService : ICommentService
    {
        private readonly HttpClient _httpClient;

        public CommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<AdminCommentListDto>> GetAdminCommentsAsync(
            CancellationToken cancellationToken = default)
        {
            var comments = await _httpClient.GetFromJsonAsync<List<AdminCommentListDto>>(
                "api/comments/admin",
                cancellationToken);

            if (comments == null)
                return Array.Empty<AdminCommentListDto>();

            return comments;
        }
    }
}
