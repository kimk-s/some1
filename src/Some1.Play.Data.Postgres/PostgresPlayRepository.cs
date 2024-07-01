using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using Npgsql;
using NpgsqlTypes;
using Utf8StringInterpolation;

namespace Some1.Play.Data.Postgres
{
    public sealed class PostgresPlayRepository : IPlayRepository
    {
        private readonly NpgsqlDataSource _dataSource;
        private readonly LoadUserParameter _loadUserParameter = new();
        private readonly SaveUserParameter _saveUserParameter = new();

        public PostgresPlayRepository(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public Task<PlayUserData[]> LoadUserAsync(IReadOnlyList<string> ids, string playId, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var param = _loadUserParameter;
                var p1 = param.Get(ids, playId);
                try
                {
                    await using var command = _dataSource.CreateCommand("SELECT * FROM get_play_users(@p1, @p2)");
                    command.Parameters.AddWithValue("p1", NpgsqlDbType.Text, p1);
                    command.Parameters.AddWithValue("p2", playId);
                    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                    var result = new PlayUserData[ids.Count];
                    int i = 0;
                    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                    {
                        result[i++] = new()
                        {
                            Id = reader.GetString(0),
                            PlayId = reader.GetString(1),
                            IsPlaying = reader.GetBoolean(2),
                            Manager = reader.GetBoolean(3),
                            Premium = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                            PackedData = reader.GetFieldValue<PackedData>(5),
                        };
                    }
                    return result;
                }
                finally
                {
                    param.Release();
                }
            }, cancellationToken);
        }

        public async Task<PlaySaveUserResult[]> SaveUserAsync(IReadOnlyList<IPlayUserData> users, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                var param = _saveUserParameter;
                var p = param.Get(users, cancellationToken);
                try
                {
                    await using var command = _dataSource.CreateCommand("SELECT * FROM set_play_users(@p)");
                    command.Parameters.AddWithValue("p", NpgsqlDbType.Text | NpgsqlDbType.Array, p);
                    await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                    var results = new PlaySaveUserResult[users.Count];
                    int n = 0;
                    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                    {
                        results[n++] = new(
                            reader.GetString(0),
                            reader.IsDBNull(1) ? null : reader.GetDateTime(1));
                    }

                    return results;
                }
                finally
                {
                    param.Release();
                }
            }, cancellationToken);
        }

        public Task<PlayPlayData> GetPlayAsync(string id, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                await using var command = _dataSource.CreateCommand("SELECT * FROM get_play_play(@p)");
                command.Parameters.AddWithValue("p", id);
                await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                await reader.ReadAsync(cancellationToken).ConfigureAwait(false);
                return new PlayPlayData()
                {
                    Maintenance = reader.GetBoolean(0),
                };
            }, cancellationToken);
        }

        public Task SetPlayAsync(PlayPlayData play, CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                await using var command = _dataSource.CreateCommand("SELECT * FROM set_play_play(@p1, @p2)");
                command.Parameters.AddWithValue("p1", play.Id);
                command.Parameters.AddWithValue("p2", play.Busy);
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }, cancellationToken);
        }

        private class LoadUserParameter
        {
            private readonly ArrayBufferWriter<byte> _buffer = new();
            private int _state;

            public ReadOnlyMemory<byte> Get(IReadOnlyList<string> ids, string playId)
            {
                if (Interlocked.CompareExchange(ref _state, 1, 0) != 0)
                {
                    throw new InvalidOperationException("Can't Mutliple Start");
                }

                try
                {
                    var buffer = _buffer;

                    buffer.ResetWrittenCount();

                    var writer = Utf8String.CreateWriter(buffer);
                    try
                    {
                        int count = ids.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var id = ids[i];
                            var separator = i < 1 ? "" : ",";
                            writer.AppendFormat($"{separator}('{id}','{playId}',true)");
                        }
                    }
                    finally
                    {
                        writer.Dispose();
                    }

                    return buffer.WrittenMemory;
                }
                catch
                {
                    Release();
                    throw;
                }
            }

            public void Release()
            {
                Volatile.Write(ref _state, 0);
            }
        }

        private class SaveUserParameter
        {
            private readonly List<ArrayBufferWriter<byte>> _buffers = new();
            private readonly List<Utf8JsonWriter> _jsonWriters = new();
            private int _state;

            public ReadOnlyMemory<byte>[] Get(IReadOnlyList<IPlayUserData> users, CancellationToken cancellationToken)
            {
                if (Interlocked.CompareExchange(ref _state, 1, 0) != 0)
                {
                    throw new InvalidOperationException("Can't Mutliple Start");
                }

                try
                {
                    var buffers = _buffers;
                    var jsonWriters = _jsonWriters;

                    Debug.Assert(buffers.Count == jsonWriters.Count);
                    if (buffers.Count < users.Count)
                    {
                        int extraCount = users.Count - buffers.Count;

                        for (int i = 0; i < extraCount; i++)
                        {
                            var buffer = new ArrayBufferWriter<byte>();
                            var jsonWriter = new Utf8JsonWriter(buffer);

                            buffers.Add(buffer);
                            jsonWriters.Add(jsonWriter);
                        }
                    }

                    var insertValues = new ReadOnlyMemory<byte>[users.Count];

                    Parallel.ForEach(
                        Partitioner.Create(0, users.Count),
                        new ParallelOptions()
                        {
                            CancellationToken = cancellationToken
                        },
                        source =>
                        {
                            var buffers = _buffers;
                            var jsonWriters = _jsonWriters;

                            for (int i = source.Item1; i < source.Item2; i++)
                            {
                                var user = users[i];
                                var buffer = buffers[i];
                                var jsonWriter = jsonWriters[i];

                                buffer.ResetWrittenCount();
                                jsonWriter.Reset();

                                var writer = Utf8String.CreateWriter(buffer);
                                try
                                {
                                    writer.AppendFormat($"('{user.Id}','{user.PlayId}',{user.IsPlaying},'");
                                    JsonSerializer.Serialize(jsonWriter, user.PackedData);
                                    writer.Append("')");
                                }
                                finally
                                {
                                    writer.Dispose();
                                }

                                insertValues[i] = buffer.WrittenMemory;
                            }
                        });

                    return insertValues;
                }
                catch
                {
                    Release();
                    throw;
                }
            }

            public void Release()
            {
                Volatile.Write(ref _state, 0);
            }
        }
    }
}
