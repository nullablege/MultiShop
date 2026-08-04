using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.CargoOperationDTOs;
using MultiShop.Cargo.EntityLayer.Concrete;
using MultiShop.Cargo.WebApi.Authorization;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = CargoAuthorizationConstants.ManagementPolicy)]

    public class CargoOperationsController : ControllerBase
    {
        private readonly ICargoOperationService _cargoOperationService;

        public CargoOperationsController(ICargoOperationService cargoOperationService)
        {
            _cargoOperationService = cargoOperationService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var cargoOperations = await _cargoOperationService.GetAllAsync(cancellationToken);
            return Ok(cargoOperations);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CargoOperation>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var cargoOperation = await _cargoOperationService.GetByIdAsync(id, cancellationToken);
            if (cargoOperation is null)
            {
                return NotFound();
            }

            return Ok(cargoOperation);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CreateCargoOperationDto createCargoOperationDto, CancellationToken cancellationToken = default)
        {
            var cargoOperation = new CargoOperation
            {
                Barcode = createCargoOperationDto.Barcode,
                Description = createCargoOperationDto.Description,
                OperationDate = createCargoOperationDto.OperationDate
            };

            await _cargoOperationService.CreateAsync(cargoOperation, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAsync(int id, UpdateCargoOperationDto updateCargoOperationDto, CancellationToken cancellationToken = default)
        {
            if (id != updateCargoOperationDto.CargoOperationId)
            {
                return BadRequest();
            }

            var cargoOperation = await _cargoOperationService.GetByIdAsync(id, cancellationToken);
            if (cargoOperation is null)
            {
                return NotFound();
            }

            cargoOperation.Barcode = updateCargoOperationDto.Barcode;
            cargoOperation.Description = updateCargoOperationDto.Description;
            cargoOperation.OperationDate = updateCargoOperationDto.OperationDate;

            var updated = await _cargoOperationService.UpdateAsync(cargoOperation, cancellationToken);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var cargoOperation = await _cargoOperationService.GetByIdAsync(id, cancellationToken);
            if (cargoOperation is null)
            {
                return NotFound();
            }

            var deleted = await _cargoOperationService.DeleteAsync(cargoOperation, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
