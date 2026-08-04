using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.BusinessLayer.Concrete
{
    public class CargoCustomerManager : ICargoCustomerService
    {
        private readonly ICargoCustomerDal _cargoCustomerDal;

        public CargoCustomerManager(ICargoCustomerDal cargoCustomerDal)
        {
            _cargoCustomerDal = cargoCustomerDal;
        }

        public async Task<IReadOnlyList<CargoCustomer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _cargoCustomerDal.GetAllAsync(cancellationToken);
        }

        public async Task<CargoCustomer?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _cargoCustomerDal.GetByIdAsync(id, cancellationToken);
        }

        public async Task<CargoCustomer> CreateAsync(CargoCustomer entity, CancellationToken cancellationToken = default)
        {
            return await _cargoCustomerDal.CreateAsync(entity, cancellationToken);
        }

        public async Task<bool> UpdateAsync(CargoCustomer entity, CancellationToken cancellationToken = default)
        {
            return await _cargoCustomerDal.UpdateAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteAsync(CargoCustomer entity, CancellationToken cancellationToken = default)
        {
            return await _cargoCustomerDal.DeleteAsync(entity, cancellationToken);
        }
    }
}
