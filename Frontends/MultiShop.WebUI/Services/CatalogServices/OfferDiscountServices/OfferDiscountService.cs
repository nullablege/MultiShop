using MultiShop.WebUI.Models.Catalog.OfferDiscountDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.OfferDiscountServices
{
    public sealed class OfferDiscountService : IOfferDiscountService
    {
        private readonly HttpClient _httpClient;

        public OfferDiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(CreateOfferDiscountDto createOfferDiscountDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PostAsJsonAsync("api/offerdiscounts", createOfferDiscountDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string offerDiscountId, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync("api/offerdiscounts/" + offerDiscountId, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<ResultOfferDiscountDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultOfferDiscountDto>>("api/offerdiscounts", cancellationToken);
            if (result == null)
                return Array.Empty<ResultOfferDiscountDto>();

            return result;
        }

        public async Task<UpdateOfferDiscountDto?> GetByIdAsync(string offerDiscountId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<UpdateOfferDiscountDto>("api/offerdiscounts/" + offerDiscountId, cancellationToken);
        }

        public async Task UpdateAsync(UpdateOfferDiscountDto updateOfferDiscountDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PutAsJsonAsync("api/offerdiscounts/" + updateOfferDiscountDto.OfferDiscountId, updateOfferDiscountDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }
    }
}
