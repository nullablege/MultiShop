using MultiShop.WebUI.Models.Catalog.SpecialOfferDTOs;

namespace MultiShop.WebUI.Services.CatalogServices.SpecialOfferServices
{
    public sealed class SpecialOfferService : ISpecialOfferService
    {
        private readonly HttpClient _httpClient;

        public SpecialOfferService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAsync(CreateSpecialOfferDto createSpecialOfferDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PostAsJsonAsync("specialoffers", createSpecialOfferDto, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string specialOfferId, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.DeleteAsync("specialoffers/" + specialOfferId, cancellationToken);
            result.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<ResultSpecialOfferDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.GetFromJsonAsync<List<ResultSpecialOfferDto>>("specialoffers", cancellationToken);
            if (result == null)
                return Array.Empty<ResultSpecialOfferDto>();

            return result;
        }

        public async Task<UpdateSpecialOfferDto?> GetByIdAsync(string specialOfferId, CancellationToken cancellationToken = default)
        {
            return await _httpClient.GetFromJsonAsync<UpdateSpecialOfferDto>("specialoffers/" + specialOfferId, cancellationToken);
        }

        public async Task UpdateAsync(UpdateSpecialOfferDto updateSpecialOfferDto, CancellationToken cancellationToken = default)
        {
            var result = await _httpClient.PutAsJsonAsync(
                "specialoffers/" + updateSpecialOfferDto.SpecialOfferId,
                updateSpecialOfferDto,
                cancellationToken);

            result.EnsureSuccessStatusCode();
        }
    }
}
