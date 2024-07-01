using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Some1.Play.Core.Paralleling;

namespace Some1.Play.Core.Internal
{
    internal sealed class LeaderGroup : IDisposable
    {
        private readonly ILeaderWorker _worker;
        private readonly Leader[] _all;

        internal LeaderGroup(IEnumerable<Leader> all, Paralleling.ParallelOptions parallelOptions)
        {
            _all = all.ToArray();
            _worker = LeaderWorker.Create(_all, new ParallelTokenGroup(parallelOptions));
        }

        public IReadOnlyList<ILeader> All => _all;

        internal void Lead1()
        {
            _worker.Lead(1);
        }

        internal void Lead2()
        {
            _worker.Lead(2);
        }

        internal void Lead3()
        {
            _worker.Lead(3);
        }

        public void Dispose()
        {
            _worker.Dispose();
        }

        private static class LeaderWorker
        {
            public static ILeaderWorker Create(Leader[] all, ParallelTokenGroup parallelTokens)
            {
                return parallelTokens.All.Count > 1
                    //? new ThreadLeaderWorker(all, parallelTokens)
                    ? new ParallelLeaderWorker(all, parallelTokens)
                    : new JustForLeaderWorker(all, parallelTokens);
            }
        }

        private interface ILeaderWorker : IDisposable
        {
            void Lead(int leadNumber);
        }

        private class ParallelLeaderWorker : ILeaderWorker
        {
            private readonly Leader[] _all;
            private readonly CancellationTokenSource _cts = new();
            private readonly ConcurrentQueue<ParallelToken> _parallelTokens;
            private readonly int _parallelTokenCount;
            private bool _disposed;
            private int _leadNumber;

            public ParallelLeaderWorker(Leader[] all, ParallelTokenGroup parallelTokens)
            {
                _all = all;
                _parallelTokens = new(parallelTokens.All);
                _parallelTokenCount = parallelTokens.All.Count;
            }

            public void Dispose()
            {
                if (_disposed)
                {
                    return;
                }

                _cts.Cancel();
                _cts.Dispose();

                _disposed = true;
            }

            public void Lead(int leadNumber)
            {
                _leadNumber = leadNumber;

                Parallel.ForEach(
                    Partitioner.Create(0, _all.Length),
                    new System.Threading.Tasks.ParallelOptions()
                    {
                        MaxDegreeOfParallelism = _parallelTokenCount,
                        CancellationToken = _cts.Token
                    },
                    LocalInit,
                    Body,
                    LocalFinally);

                _leadNumber = 0;
            }

            private ParallelToken LocalInit()
            {
                if (!_parallelTokens.TryDequeue(out var parallelToken))
                {
                    throw new InvalidOperationException("Failed to TryDequeue ParallelToken.");
                }
                return parallelToken;
            }

            private ParallelToken Body(Tuple<int, int> source, ParallelLoopState loopState, ParallelToken parallelToken)
            {
                foreach (var item in _all.AsSpan(source.Item1, source.Item2 - source.Item1))
                {
                    item.Lead(_leadNumber, parallelToken);

                    //_cts.Token.ThrowIfCancellationRequested();
                }

                return parallelToken;
            }

            private void LocalFinally(ParallelToken parallelToken)
            {
                _parallelTokens.Enqueue(parallelToken);
            }
        }

        private class JustForLeaderWorker : ILeaderWorker
        {
            private readonly Leader[] _all;
            private readonly ParallelToken _parallelToken;

            public JustForLeaderWorker(Leader[] all, ParallelTokenGroup parallelTokens)
            {
                _all = all;
                _parallelToken = parallelTokens.All.Single();
            }

            public void Dispose()
            {
            }

            public void Lead(int leadNumber)
            {
                foreach (var item in _all.AsSpan())
                {
                    item.Lead(leadNumber, _parallelToken);
                }
            }
        }

        //private class ThreadLeaderWorker : ILeaderWorker
        //{
        //    private readonly Leader[] _all;
        //    private readonly ParallelTokenGroup _parallelTokens;
        //    private readonly BlockingCollection<(int, int)> _collection;
        //    private bool _disposed;
        //    private int _leadNumber;
        //    private int _consumed;

        //    public ThreadLeaderWorker(Leader[] all, ParallelTokenGroup parallelTokens)
        //    {
        //        if (all.Length == 0)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(all));
        //        }

        //        if (parallelTokens.All.Count == 0)
        //        {
        //            throw new ArgumentOutOfRangeException(nameof(parallelTokens));
        //        }

        //        _all = all;
        //        _parallelTokens = parallelTokens;
        //        _collection = new();

        //        for (int i = 0; i < parallelTokens.All.Count; i++)
        //        {
        //            var thread = new Thread(Work)
        //            {
        //                Name = $"{typeof(ThreadLeaderWorker).Name}-{i + 1}",
        //                IsBackground = true,
        //                Priority = ThreadPriority.Normal,
        //            };
        //            thread.Start(parallelTokens.All[i]);
        //        }
        //    }

        //    private void Work(object? obj)
        //    {
        //        var parallelToken = (ParallelToken)(obj ?? throw new ArgumentNullException());

        //        while (!_collection.IsAddingCompleted)
        //        {
        //            var source = _collection.Take();

        //            int count = source.Item2 - source.Item1;
        //            foreach (var item in _all.AsSpan(source.Item1, count))
        //            {
        //                item.Lead(_leadNumber, parallelToken);
        //            }

        //            Interlocked.Add(ref _consumed, count);
        //        }
        //    }

        //    public void Dispose()
        //    {
        //        if (_disposed)
        //        {
        //            return;
        //        }

        //        _collection.CompleteAdding();
        //        _collection.Dispose();

        //        _disposed = true;
        //    }

        //    public void Lead(int leadNumber)
        //    {
        //        _leadNumber = leadNumber;

        //        int partition = Math.Max(_all.Length / _parallelTokens.All.Count / 10, 1);

        //        int produced = 0;
        //        while (produced < _all.Length)
        //        {
        //            var source = (produced, Math.Min(produced + partition, _all.Length));
        //            _collection.Add(source);
        //            produced = source.Item2;
        //        }

        //        while (Volatile.Read(ref _consumed) < produced && !Volatile.Read(ref _disposed))
        //        {
        //        }

        //        _leadNumber = 0;
        //        _consumed = 0;
        //    }
        //}
    }
}
