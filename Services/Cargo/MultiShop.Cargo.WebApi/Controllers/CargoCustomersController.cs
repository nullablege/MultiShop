using Microsoft.AspNetCore.Mvc;
using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DtoLayer.CargoCustomerDTOs;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoCustomersController : ControllerBase
    {
        private readonly ICargoCustomerService _cargoCustomerService;

        public CargoCustomersController(ICargoCustomerService cargoCustomerService)
        {
            _cargoCustomerService = cargoCustomerService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var cargoCustomers = await _cargoCustomerService.GetAllAsync(cancellationToken);
            return Ok(cargoCustomers);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CargoCustomer>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var cargoCustomer = await _cargoCustomerService.GetByIdAsync(id, cancellationToken);
            if (cargoCustomer is null)
            {
                return NotFound();
            }

            return Ok(cargoCustomer);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(CreateCargoCustomerDto createCargoCustomerDto, CancellationToken cancellationToken = default)
        {
            var cargoCustomer = new CargoCustomer
            {
                Name = createCargoCustomerDto.Name,
                Surname = createCargoCustomerDto.Surname,
                Email = createCargoCustomerDto.Email,
                Phone = createCargoCustomerDto.Phone,
                District = createCargoCustomerDto.District,
                City = createCargoCustomerDto.City,
                Address = createCargoCustomerDto.Address,
                UserCustomerId = createCargoCustomerDto.UserCustomerId
            };

            await _cargoCustomerService.CreateAsync(cargoCustomer, cancellationToken);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateAsync(int id, UpdateCargoCustomerDto updateCargoCustomerDto, CancellationToken cancellationToken = default)
        {
            if (id != updateCargoCustomerDto.CargoCustomerId)
            {
                return BadRequest();
            }

            var cargoCustomer = await _cargoCustomerService.GetByIdAsync(id, cancellationToken);
            if (cargoCustomer is null)
            {
                return NotFound();
            }

            cargoCustomer.Name = updateCargoCustomerDto.Name;
            cargoCustomer.Surname = updateCargoCustomerDto.Surname;
            cargoCustomer.Email = updateCargoCustomerDto.Email;
            cargoCustomer.Phone = updateCargoCustomerDto.Phone;
            cargoCustomer.District = updateCargoCustomerDto.District;
            cargoCustomer.City = updateCargoCustomerDto.City;
            cargoCustomer.Address = updateCargoCustomerDto.Address;
            cargoCustomer.UserCustomerId = updateCargoCustomerDto.UserCustomerId;

            var updated = await _cargoCustomerService.UpdateAsync(cargoCustomer, cancellationToken);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var cargoCustomer = await _cargoCustomerService.GetByIdAsync(id, cancellationToken);
            if (cargoCustomer is null)
            {
                return NotFound();
            }

            var deleted = await _cargoCustomerService.DeleteAsync(cargoCustomer, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
