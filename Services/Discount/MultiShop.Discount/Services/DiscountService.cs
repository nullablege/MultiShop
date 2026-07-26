using Dapper;
using MultiShop.Discount.Data;
using MultiShop.Discount.Dtos;

namespace MultiShop.Discount.Services
{
    public sealed class DiscountService : IDiscountService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DiscountService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<ResultDiscountCouponDto>> GetAllDiscountCouponAsync(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT CouponId, Code, Rate, IsActive, ValidDate FROM dbo.Coupons;";
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);

            await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);

            var discounts = await connection.QueryAsync<ResultDiscountCouponDto>(command);
            return discounts.AsList();

        }

        public async Task<GetByIdDiscountCouponDto?> GetByIdDiscountCouponAsync(int couponId, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT CouponId, Code, Rate, IsActive, ValidDate FROM dbo.Coupons WHERE CouponId=@couponId;";
            var command = new CommandDefinition(sql, new {couponId=couponId},cancellationToken: cancellationToken);
            await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
            var discount = await connection.QuerySingleOrDefaultAsync<GetByIdDiscountCouponDto>(command);
            return discount;
        }

        public async Task<ResultDiscountCouponDto?> GetCodeDetailByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT CouponId, Code, Rate, IsActive, ValidDate FROM dbo.Coupons WHERE Code=@code;";
            var command = new CommandDefinition(sql, new {code=code},cancellationToken: cancellationToken);
            await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
            var discount = await connection.QuerySingleOrDefaultAsync<ResultDiscountCouponDto>(command);
            return discount;
        }

        public async Task CreateDiscountCouponAsync(CreateDiscountCouponDto createDiscountCouponDto, CancellationToken cancellationToken = default)
        {
            const string sql = "insert into dbo.Coupons(Code, Rate, IsActive, ValidDate) values(@Code, @Rate, @IsActive, @ValidDate)";
            var command = new CommandDefinition(sql, createDiscountCouponDto, cancellationToken: cancellationToken);
            await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
            var affectedRows = await connection.ExecuteAsync(command);

            if(affectedRows != 1)
            {
                throw new InvalidOperationException("Kupon Oluşturulamadı");
            }
        }

        public async Task<bool> UpdateDiscountCouponAsync(UpdateDiscountCouponDto updateDiscountCouponDto, CancellationToken cancellationToken = default)
        {
            const string sql = "UPDATE dbo.Coupons SET Code=@Code, Rate=@Rate, IsActive=@IsActive, ValidDate=@ValidDate WHERE CouponId=@CouponId;";
            var command = new CommandDefinition(sql, updateDiscountCouponDto, cancellationToken: cancellationToken);
            await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
            var affectedRow = await connection.ExecuteAsync(command);
            if(affectedRow == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteDiscountCouponAsync(int couponId, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE from dbo.Coupons WHERE CouponId=@couponId;";
            var command = new CommandDefinition(sql, new {couponId=couponId}, cancellationToken: cancellationToken);
            await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
            var affectedRow = await connection.ExecuteAsync(command);
            if(affectedRow == 0)
            {
                return false;
            }
            return true;

        }

        public async Task<int> GetDiscountCouponCountAsync(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT count(*) FROM dbo.Coupons;";
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
            await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
            var result = await connection.QuerySingleAsync<int>(command);
            return result;
        }

        public async Task<int?> GetDiscountCouponRateByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT Rate FROM dbo.Coupons WHERE Code=@Code";
            var command = new CommandDefinition(sql, new {Code=code}, cancellationToken: cancellationToken);
            await using var connection = await _connectionFactory.OpenConnectionAsync(cancellationToken);
            var result = await connection.QuerySingleOrDefaultAsync<int?>(command);
            return result;
        }
    }
}
