using MultiShop.WebUI.Models.CommentDTOs;
using System.Net;

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

        public async Task<AdminCommentListDto?> GetAdminCommentAsync(
            int commentId,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(
                $"api/comments/admin/{commentId}",
                cancellationToken);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<AdminCommentListDto>(
                cancellationToken);
        }

        public async Task UpdateStatusAsync(
            int commentId,
            UpdateCommentStatusDto updateCommentStatusDto,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/comments/admin/{commentId}/status",
                updateCommentStatusDto,
                cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(
            int commentId,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/comments/admin/{commentId}",
                cancellationToken);

            response.EnsureSuccessStatusCode();
        }
    }
}
