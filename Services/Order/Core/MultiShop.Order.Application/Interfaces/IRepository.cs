using System.Linq.Expressions;

namespace MultiShop.Order.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetByFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    }
}
