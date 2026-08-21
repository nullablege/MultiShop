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
            var result = await _httpClient.PostAsJsonAsync("offerdiscounts", createOfferDiscountDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string offerDiscountId, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync("offerdiscounts/" + offerDiscountId, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<ResultOfferDiscountDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultOfferDiscountDto>>("offerdiscounts", cancellationToken);
            if (result == null)
                return Array.Empty<ResultOfferDiscountDto>();

            return result;
        }

        public async Task<UpdateOfferDiscountDto?> GetByIdAsync(string offerDiscountId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<UpdateOfferDiscountDto>("offerdiscounts/" + offerDiscountId, cancellationToken);
        }

        public async Task UpdateAsync(UpdateOfferDiscountDto updateOfferDiscountDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PutAsJsonAsync("offerdiscounts/" + updateOfferDiscountDto.OfferDiscountId, updateOfferDiscountDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }
    }
}
