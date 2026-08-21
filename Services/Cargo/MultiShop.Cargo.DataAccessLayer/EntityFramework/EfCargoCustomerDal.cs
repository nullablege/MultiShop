using Microsoft.EntityFrameworkCore;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Context;
using MultiShop.Cargo.DataAccessLayer.Repositories;
using MultiShop.Cargo.EntityLayer.Concrete;

namespace MultiShop.Cargo.DataAccessLayer.EntityFramework
{
    public class EfCargoCustomerDal : GenericRepository<CargoCustomer>, ICargoCustomerDal
    {
        private readonly CargoContext _cargoContext;

        public EfCargoCustomerDal(CargoContext cargoContext) : base(cargoContext)
        {
            _cargoContext = cargoContext;
        }

        public async Task<CargoCustomer?> GetByUserIdAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            return await _cargoContext.CargoCustomers
                .AsNoTracking()
                .OrderBy(customer => customer.CargoCustomerId)
                .FirstOrDefaultAsync(
                    customer => customer.UserCustomerId == userId,
                    cancellationToken);
        }
    }
}
