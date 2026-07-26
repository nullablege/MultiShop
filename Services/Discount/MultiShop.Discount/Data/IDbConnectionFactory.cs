using System.Data.Common;

namespace MultiShop.Discount.Data
{
    public interface IDbConnectionFactory
    {
        Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default);
    }
}
