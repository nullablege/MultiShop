using MultiShop.WebUI.Models.Catalog.AboutDTOs;
namespace MultiShop.WebUI.Services.CatalogServices.AboutServices;
public interface IAboutService { Task<IReadOnlyList<ResultAboutDto>> GetAllAsync(CancellationToken cancellationToken = default); Task<UpdateAboutDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default); Task CreateAsync(CreateAboutDto dto, CancellationToken cancellationToken = default); Task UpdateAsync(UpdateAboutDto dto, CancellationToken cancellationToken = default); Task DeleteAsync(string id, CancellationToken cancellationToken = default); }
