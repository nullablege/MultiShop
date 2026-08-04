using Microsoft.EntityFrameworkCore;
using MultiShop.Cargo.DataAccessLayer.Abstract;
using MultiShop.Cargo.DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly CargoContext _cargoContext;

        public GenericRepository(CargoContext cargoContext)
        {
            _cargoContext = cargoContext;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _cargoContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _cargoContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _cargoContext.Set<T>().AddAsync(entity, cancellationToken);
            await _cargoContext.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _cargoContext.Set<T>().Update(entity);
            return await _cargoContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _cargoContext.Set<T>().Remove(entity);
            return await _cargoContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
