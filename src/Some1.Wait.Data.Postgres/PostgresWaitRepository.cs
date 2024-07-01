using Npgsql;
using NpgsqlTypes;

namespace Some1.Wait.Data.Postgres
{
    public sealed class PostgresWaitRepository : IWaitRepository
    {
        private readonly NpgsqlDataSource _dataSource;

        public PostgresWaitRepository(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<WaitUserData> GetUserAsync(string id, CancellationToken cancellationToken)
        {
            await using var command = _dataSource.CreateCommand("SELECT * FROM get_wait_user(@p)");
            command.Parameters.AddWithValue("p", id);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            return new()
            {
                Id = reader.GetString(0),
                PlayId = reader.GetString(1),
                IsPlaying = reader.GetBoolean(2),
                Manager = reader.GetBoolean(3),
            };
        }

        public async Task<WaitWaitData> GetWaitAsync(CancellationToken cancellationToken)
        {
            await using var command = _dataSource.CreateCommand("SELECT * FROM get_wait_wait()");
            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            return new()
            {
                Maintenance = reader.GetBoolean(0),
            };
        }

        public async Task<WaitPlayData[]> GetPlaysAsync(CancellationToken cancellationToken)
        {
            await using var command = _dataSource.CreateCommand("SELECT * FROM get_wait_plays()");
            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            var result = new List<WaitPlayData>();
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                result.Add(new()
                {
                    Id = reader.GetString(0),
                    Region = reader.GetString(1),
                    City = reader.GetString(2),
                    Number = reader.GetInt32(3),
                    Address = reader.GetString(4),
                    OpeningSoon = reader.GetBoolean(5),
                    Maintenance = reader.GetBoolean(6),
                    Busy = reader.GetFloat(7),
                });
            }
            return result.ToArray();
        }

        public async Task<WaitPremiumLogData[]> GetPremiumLogsAsync(string userId, CancellationToken cancellationToken)
        {
            await using var command = _dataSource.CreateCommand("SELECT * FROM get_wait_premium_logs(@p)");
            command.Parameters.AddWithValue("p", NpgsqlDbType.Varchar, userId);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            var result = new List<WaitPremiumLogData>();
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                result.Add(new()
                {
                    Id = reader.GetGuid(0),
                    UserId = reader.GetString(1),
                    Reason = (WaitPremiumLogReason)reader.GetInt32(2),
                    CreatedDate = reader.GetDateTime(3),
                    PremiumChangedDays = reader.GetInt32(4),
                    PremiumExpirationDate = reader.GetDateTime(5),
                    PurchaseOrderId = reader.IsDBNull(6) ? null : reader.GetString(6),
                    PurchaseProductId = reader.IsDBNull(7) ? null : reader.GetString(7),
                    PurchaseDate = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                    Note = reader.IsDBNull(9) ? null : reader.GetString(9),
                });
            }
            return result.ToArray();
        }

        public async Task BuyProductAsync(string userId, string productId, string orderId, DateTime purchaseDate, int premium, CancellationToken cancellationToken)
        {
            await using var command = _dataSource.CreateCommand("SELECT * FROM buy_wait_product(@p1, @p2, @p3, @p4, @p5)");
            command.Parameters.AddWithValue("p1", NpgsqlDbType.Varchar, userId);
            command.Parameters.AddWithValue("p2", NpgsqlDbType.Varchar, productId);
            command.Parameters.AddWithValue("p3", NpgsqlDbType.Varchar, orderId);
            command.Parameters.AddWithValue("p4", NpgsqlDbType.Timestamp, purchaseDate);
            command.Parameters.AddWithValue("p5", premium);
            await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
