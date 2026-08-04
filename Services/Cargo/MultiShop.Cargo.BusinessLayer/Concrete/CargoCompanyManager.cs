using MultiShop.Cargo.BusinessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.BusinessLayer.Concrete
{
    public class CargoCompanyManager : ICargoCompanyService
    {
        private readonly ICargoCompanyDal _cargoCompanyDal;
        public CargoCompanyManager(ICargoCompanyDal cargoCompanyDal)
        {
            _cargoCompanyDal = cargoCompanyDal;
        }

        public async Task<IReadOnlyList<CargoCompany>> GetAllAsync(CancellationToken cancellationToken = default)
        {
           return await _cargoCompanyDal.GetAllAsync(cancellationToken);
        }

        public async Task<CargoCompany?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _cargoCompanyDal.GetByIdAsync(id, cancellationToken);
        }

        public async Task<CargoCompany> CreateAsync(CargoCompany entity, CancellationToken cancellationToken = default)
        {
            return await _cargoCompanyDal.CreateAsync(entity, cancellationToken);
        }

        public async Task<bool> UpdateAsync(CargoCompany entity, CancellationToken cancellationToken = default)
        {
            return await _cargoCompanyDal.UpdateAsync(entity, cancellationToken);
        }

        public async Task<bool> DeleteAsync(CargoCompany entity, CancellationToken cancellationToken = default)
        {
            return await _cargoCompanyDal.DeleteAsync(entity, cancellationToken);
        }
    }
}
