using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.BusinessLayer.Concrete
{
    public class CargoDetailManager : ICargoDetailService
    {
        private readonly ICargoDetailDal _cargoDetailDal;

        public CargoDetailManager(ICargoDetailDal cargoDetailDal)
        {
            _cargoDetailDal = cargoDetailDal;
        }

        public async Task<IReadOnlyList<CargoDetail>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _cargoDetailDal.GetAllAsync(cancellationToken);
        }

        public async Task<CargoDetail?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _cargoDetailDal.GetByIdAsync(id, cancellationToken);
        }

        public async Task<CargoDetail> CreateAsync(CargoDetail entity, CancellationToken cancellationToken = default)
        {
            return await _cargoDetailDal.CreateAsync(entity, cancellationToken);
        }

        public async Task<bool> UpdateAsync(CargoDetail entity, CancellationToken cancellationToken = default)
        {
            return await _cargoDetailDal.UpdateAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteAsync(CargoDetail entity, CancellationToken cancellationToken = default)
        {
            return await _cargoDetailDal.DeleteAsync(entity, cancellationToken);
        }
    }
}
