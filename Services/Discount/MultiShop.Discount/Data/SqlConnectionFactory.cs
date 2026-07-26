using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MultiShop.Discount.Configuration;
using System.Data.Common;

namespace MultiShop.Discount.Data
{
    public sealed class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(IOptions<SqlServerOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public async Task<DbConnection> OpenConnectionAsync(CancellationToken cancellationToken = default)
        {
            var conn = new SqlConnection(_connectionString);
            try
            {
                await conn.OpenAsync(cancellationToken);
                return conn;
            }
            catch
            {
                await conn.DisposeAsync();
                throw;
            }

        }
    }
}
