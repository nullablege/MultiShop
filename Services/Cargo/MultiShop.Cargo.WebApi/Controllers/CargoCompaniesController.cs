using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.CargoCompanyDTOs;
using MultiShop.Cargo.EntityLayer.Concrete;
using MultiShop.Cargo.WebApi.Authorization;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = CargoAuthorizationConstants.ManagementPolicy)]
    public class CargoCompaniesController : ControllerBase
    {
        private readonly ICargoCompanyService _cargoCompanyService;

        public CargoCompaniesController(ICargoCompanyService cargoCompanyService)
        {
            _cargoCompanyService = cargoCompanyService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var cargoCompanies = await _cargoCompanyService.GetAllAsync(cancellationToken);
            return Ok(cargoCompanies);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CargoCompany>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var cargoCompany = await _cargoCompanyService.GetByIdAsync(id, cancellationToken);
            if (cargoCompany is null)
            {
                return NotFound();
            }

            return Ok(cargoCompany);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CreateCargoCompanyDto createCargoCompanyDto, CancellationToken cancellationToken = default)
        {
            var cargoCompany = new CargoCompany
            {
                CargoCompanyName = createCargoCompanyDto.CargoCompanyName
            };

            await _cargoCompanyService.CreateAsync(cargoCompany, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAsync(int id, UpdateCargoCompanyDto updateCargoCompanyDto, CancellationToken cancellationToken = default)
        {
            if (id != updateCargoCompanyDto.CargoCompanyId)
            {
                return BadRequest();
            }

            var cargoCompany = await _cargoCompanyService.GetByIdAsync(id, cancellationToken);
            if (cargoCompany is null)
            {
                return NotFound();
            }

            cargoCompany.CargoCompanyName = updateCargoCompanyDto.CargoCompanyName;

            var updated = await _cargoCompanyService.UpdateAsync(cargoCompany, cancellationToken);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var cargoCompany = await _cargoCompanyService.GetByIdAsync(id, cancellationToken);
            if (cargoCompany is null)
            {
                return NotFound();
            }

            var deleted = await _cargoCompanyService.DeleteAsync(cargoCompany, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
