using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.BusinessLayer.Concrete
{
    public class CargoOperationManager : ICargoOperationService
    {
        private readonly ICargoOperationDal _cargoOperationDal;

        public CargoOperationManager(ICargoOperationDal cargoOperationDal)
        {
            _cargoOperationDal = cargoOperationDal;
        }

        public async Task<IReadOnlyList<CargoOperation>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _cargoOperationDal.GetAllAsync(cancellationToken);
        }

        public async Task<CargoOperation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _cargoOperationDal.GetByIdAsync(id, cancellationToken);
        }

        public async Task<CargoOperation> CreateAsync(CargoOperation entity, CancellationToken cancellationToken = default)
        {
            return await _cargoOperationDal.CreateAsync(entity, cancellationToken);
        }

        public async Task<bool> UpdateAsync(CargoOperation entity, CancellationToken cancellationToken = default)
        {
            return await _cargoOperationDal.UpdateAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteAsync(CargoOperation entity, CancellationToken cancellationToken = default)
        {
            return await _cargoOperationDal.DeleteAsync(entity, cancellationToken);
        }
    }
}
