using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MemoryPack;
using R3;

namespace Some1.Sync.Destinations
{
    public sealed class SyncArrayDestination : ISyncDestination, ISyncPolatable
    {
        public const int MaxCount = byte.MaxValue;

        private readonly ISyncDestination[] _items;
        private readonly ISyncPolatable[] _polatableItems;
        private readonly ReactiveProperty<bool> _isDefault = new();
        private int _defaultItemCount;

        public SyncArrayDestination(IEnumerable<ISyncDestination> items) : this(items.ToArray())
        {
        }

        public SyncArrayDestination(params ISyncDestination[] items)
        {
            if (items.Length > MaxCount)
            {
                throw new ArgumentOutOfRangeException(nameof(items));
            }

            _items = items;
            _polatableItems = items
                .OfType<ISyncPolatable>()
                .Where(x => (x as SyncArrayDestination)?.AnyPolatableItem ?? true)
                .ToArray();

            for (byte i = 0; i < _items.Length; i++)
            {
                byte index = i;
                var item = _items[index];

                if (item.IsDefault.CurrentValue)
                {
                    DefaultItemCount++;
                }

                item.IsDefault.Skip(1).Subscribe(x =>
                {
                    if (x)
                    {
                        DefaultItemCount++;
                    }
                    else
                    {
                        DefaultItemCount--;
                    }
                });
            }
        }

        public bool AnyPolatableItem => _polatableItems.Length > 0;

        public ReadOnlyReactiveProperty<bool> IsDefault => _isDefault;

        public int DefaultItemCount
        {
            get => _defaultItemCount;

            set
            {
                if (value < 0 || value > _items.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                if (_defaultItemCount == value)
                {
                    return;
                }

                _defaultItemCount = value;
                _isDefault.Value = value == _items.Length;
            }
        }

        public void Read(ref MemoryPackReader reader, SyncMode mode)
        {
            byte code = reader.ReadUnmanaged<byte>();
            if (code == 0)
            {
                Reset();
            }
            else if (code == 1)
            {
                if (mode == SyncMode.Full)
                {
                    foreach (var item in new ReadOnlySpan<ISyncDestination>(_items))
                    {
                        item.Read(ref reader, mode);
                    }
                }
                else
                {
                    byte count = reader.ReadUnmanaged<byte>();
                    Debug.Assert(count <= MaxCount);
                    var item = _items;
                    for (byte i = 0; i < count; i++)
                    {
                        int index = reader.ReadUnmanaged<byte>();
                        item[index].Read(ref reader, mode);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Reset()
        {
            if (IsDefault.CurrentValue)
            {
                return;
            }

            foreach (var item in new ReadOnlySpan<ISyncDestination>(_items))
            {
                item.Reset();
            }
        }

        public void Dispose()
        {
            foreach (var item in new ReadOnlySpan<ISyncDestination>(_items))
            {
                item.Dispose();
            }
            _isDefault.Dispose();
        }

        public void Extrapolate()
        {
            if (IsDefault.CurrentValue)
            {
                return;
            }

            foreach (var item in new ReadOnlySpan<ISyncPolatable>(_polatableItems))
            {
                item.Extrapolate();
            }
        }

        public void Interpolate(float time)
        {
            if (IsDefault.CurrentValue)
            {
                return;
            }

            foreach (var item in new ReadOnlySpan<ISyncPolatable>(_polatableItems))
            {
                item.Interpolate(time);
            }
        }
    }
}
