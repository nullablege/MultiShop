using System.Net;
using MultiShop.WebUI.Models.Message;

namespace MultiShop.WebUI.Services.MessageServices;

public sealed class MessageService : IMessageService
{
    private readonly HttpClient _httpClient;

    public MessageService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<InboxMessageDto>> GetInboxAsync(
        CancellationToken cancellationToken = default)
    {
        var messages = await _httpClient.GetFromJsonAsync<List<InboxMessageDto>>(
            "messages/inbox",
            cancellationToken);

        return messages ?? [];
    }

    public async Task<IReadOnlyList<SentMessageDto>> GetSentAsync(
        CancellationToken cancellationToken = default)
    {
        var messages = await _httpClient.GetFromJsonAsync<List<SentMessageDto>>(
            "messages/sent",
            cancellationToken);

        return messages ?? [];
    }

    public async Task<bool> MarkAsReadAsync(
        int messageId,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Put,
            $"messages/{messageId}/read");

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        response.EnsureSuccessStatusCode();
        return true;
    }
}
