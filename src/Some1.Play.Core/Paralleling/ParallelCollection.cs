using System;
using System.Collections;
using System.Collections.Generic;

namespace Some1.Play.Core.Paralleling
{
    public interface IReadOnlyParallelCollection<T> : IReadOnlyCollection<T>
    {
        int ParallelCount { get; }
    }

    public sealed class ParallelCollection<T> : ICollection<T>, IReadOnlyParallelCollection<T>
    {
        private readonly ListHead[] _listHeads;
        private readonly int _size;
        private int _version;
        private int _clearVersion;

        public ParallelCollection(ParallelOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var listHeads = new ListHead[options.GetSafeCount()];
            for (var i = 0; i < listHeads.Length; i++)
            {
                listHeads[i] = new(new List<T>());
            }
            _listHeads = listHeads;
            _size = listHeads.Length;
        }

        public int ParallelCount => _listHeads.Length;

        public int Count
        {
            get
            {
                var result = 0;

                foreach (var listHead in _listHeads.AsSpan())
                {
                    result += listHead.Count;
                }

                return result;
            }
        }

        bool ICollection<T>.IsReadOnly => false;

        public bool Add(T item, ParallelToken? parallelToken)
        {
            ref var listHead = ref _listHeads[parallelToken?.Index ?? 0];
            if (listHead.Count > 100_000)
            {
                return false;
            }

            listHead.Add(item);
            _version++;
            return true;
        }

        public void Add(T item)
        {
            Add(item, null);
        }

        public void Clear()
        {
            _version++;
            _clearVersion = _version;

            foreach (ref var listHead in _listHeads.AsSpan())
            {
                listHead.Clear();
            }
        }

        public bool Contains(T item)
        {
            foreach (var listHead in _listHeads.AsSpan())
            {
                if (listHead.Contains(item))
                {
                    return true;
                }
            }
            
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = 0;
            foreach (T item in this)
            {
                array[i++] = item;
            }
        }

        public Enumerator GetEnumerator() => new(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();

        public bool Remove(T item)
        {
            foreach (ref var listHead in _listHeads.AsSpan())
            {
                if (listHead.Remove(item))
                {
                    _version++;
                    return true;
                }
            }
            return false;
        }

        private struct ListHead
        {
            public byte any;
            public readonly List<T> list;

            public ListHead(List<T> list)
            {
                this.list = list;
                any = list.Count == 0 ? (byte)0 : (byte)1;
            }

            public readonly int Count => any == 0 ? 0 : list.Count;

            public void Clear()
            {
                if (any == 0)
                {
                    return;
                }

                any = 0;
                list.Clear();
            }

            public readonly bool Contains(T item)
            {
                return any != 0 && list.Contains(item);
            }

            public void Add(T item)
            {
                any = 1;
                list.Add(item);
            }

            public bool Remove(T item)
            {
                if (any == 0)
                {
                    return false;
                }

                return list.Remove(item);
            }
        }

        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly ParallelCollection<T> _collection;
            private int _index1;
            private int _index2;
            private readonly int _version;
            private T? _current;

            internal Enumerator(ParallelCollection<T> collection)
            {
                _collection = collection;
                _index1 = -1;
                _index2 = 0;
                _version = collection._version;
                _current = default;
            }

            public readonly void Dispose()
            {
            }

            public bool MoveNext()
            {
                var localCollection = _collection;

                if (_version == localCollection._clearVersion)
                {
                    return false;
                }

                if (_version == localCollection._version)
                {
                    if (_index1 < 0)
                    {
                        _index1 = 0;
                    }
                    while ((uint)_index1 < (uint)localCollection._size)
                    {
                        var listHead = localCollection._listHeads[_index1];
                        if ((uint)_index2 < (uint)listHead.Count)
                        {
                            _current = listHead.list[_index2];
                            _index2++;
                            return true;
                        }
                        _index1++;
                        _index2 = 0;
                    }
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                if (_version != _collection._version)
                {
                    throw new InvalidOperationException();
                }

                _index1 = _collection._size + 1;
                _index2 = 0;
                _current = default;
                return false;
            }

            public readonly T Current => _current!;

            readonly object? IEnumerator.Current
            {
                get
                {
                    if (_index1 == -1 || _index1 == _collection._size + 1)
                    {
                        throw new InvalidOperationException();
                    }
                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                throw new NotImplementedException();
            }
        }
    }
}
