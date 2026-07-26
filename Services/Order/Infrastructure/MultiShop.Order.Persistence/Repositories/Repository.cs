using Microsoft.EntityFrameworkCore;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Order.Persistence.Repositories
{
    public class Repository<T>:IRepository<T> where T : class
    {
        private readonly OrderDbContext _context;

        public Repository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetByFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            var values = await _context.Set<T>().Where(filter).ToListAsync(cancellationToken);
            return values;
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(new object[] {id}, cancellationToken);
        }

        public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync(cancellationToken) > 0;

        }
    }
}
