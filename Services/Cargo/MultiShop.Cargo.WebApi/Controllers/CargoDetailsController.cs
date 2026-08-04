using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.CargoDetailDTOs;
using MultiShop.Cargo.EntityLayer.Concrete;
using MultiShop.Cargo.WebApi.Authorization;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = CargoAuthorizationConstants.ManagementPolicy)]

    public class CargoDetailsController : ControllerBase
    {
        private readonly ICargoDetailService _cargoDetailService;

        public CargoDetailsController(ICargoDetailService cargoDetailService)
        {
            _cargoDetailService = cargoDetailService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var cargoDetails = await _cargoDetailService.GetAllAsync(cancellationToken);
            return Ok(cargoDetails);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CargoDetail>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var cargoDetail = await _cargoDetailService.GetByIdAsync(id, cancellationToken);
            if (cargoDetail is null)
            {
                return NotFound();
            }

            return Ok(cargoDetail);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CreateCargoDetailDto createCargoDetailDto, CancellationToken cancellationToken = default)
        {
            var cargoDetail = new CargoDetail
            {
                SenderCustomer = createCargoDetailDto.SenderCustomer,
                ReceiverCustomer = createCargoDetailDto.ReceiverCustomer,
                Barcode = createCargoDetailDto.Barcode,
                CargoCompanyId = createCargoDetailDto.CargoCompanyId
            };

            await _cargoDetailService.CreateAsync(cargoDetail, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAsync(int id, UpdateCargoDetailDto updateCargoDetailDto, CancellationToken cancellationToken = default)
        {
            if (id != updateCargoDetailDto.CargoDetailId)
            {
                return BadRequest();
            }

            var cargoDetail = await _cargoDetailService.GetByIdAsync(id, cancellationToken);
            if (cargoDetail is null)
            {
                return NotFound();
            }

            cargoDetail.SenderCustomer = updateCargoDetailDto.SenderCustomer;
            cargoDetail.ReceiverCustomer = updateCargoDetailDto.ReceiverCustomer;
            cargoDetail.Barcode = updateCargoDetailDto.Barcode;
            cargoDetail.CargoCompanyId = updateCargoDetailDto.CargoCompanyId;

            var updated = await _cargoDetailService.UpdateAsync(cargoDetail, cancellationToken);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var cargoDetail = await _cargoDetailService.GetByIdAsync(id, cancellationToken);
            if (cargoDetail is null)
            {
                return NotFound();
            }

            var deleted = await _cargoDetailService.DeleteAsync(cargoDetail, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
