using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MemoryPack;
using R3;

namespace Some1.Sync.Sources
{
    public sealed class SyncArraySource : ISyncSource
    {
        public const int MaxCount = byte.MaxValue;

        private readonly ReactiveProperty<bool> _isDefault = new();
        private readonly ReactiveProperty<bool> _dirty = new();
        private readonly ISyncSource[] _items;
        private readonly List<KeyValuePair<byte, ISyncSource>> _dirtyItems;
        private int _defaultItemCount;

        public SyncArraySource(IEnumerable<ISyncSource> items) : this(items.ToArray())
        {
        }

        public SyncArraySource(params ISyncSource[] items)
        {
            if (items.Length > MaxCount)
            {
                throw new ArgumentOutOfRangeException(nameof(items));
            }

            _items = items;
            _dirtyItems = new(items.Length);

            for (byte i = 0; i < items.Length; i++)
            {
                byte index = i;
                var item = items[index];

                if (item.IsDefault.CurrentValue)
                {
                    SetDefaultItemCount(DefaultItemCount + 1);
                }

                item.IsDefault.Skip(1).Subscribe(x =>
                {
                    if (x)
                    {
                        SetDefaultItemCount(DefaultItemCount + 1);
                    }
                    else
                    {
                        SetDefaultItemCount(DefaultItemCount - 1);
                    }
                });

                item.Dirty.Where(x => x).Subscribe(_ =>
                {
                    _dirtyItems.Add(new(index, _items[index]));
                    _dirty.Value = true;
                });
            }
        }

        public ReadOnlyReactiveProperty<bool> IsDefault => _isDefault;

        public ReadOnlyReactiveProperty<bool> Dirty => _dirty;

        public IReadOnlyList<ISyncSource> Items => _items;

        public int DefaultItemCount
        {
            get => _defaultItemCount;

            private set
            {
                if (_defaultItemCount == value)
                {
                    return;
                }

                _defaultItemCount = value;
                _isDefault.Value = value == _items.Length;
            }
        }

        private void SetDefaultItemCount(int value)
        {
            if (value < 0 || value > _items.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"{value}/{_items.Length}");
            }

            DefaultItemCount = value;
        }

        public void Write<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, SyncMode mode) where TBufferWriter :
#if UNITY
class,
#endif
IBufferWriter<byte>
        {
            if (mode == SyncMode.Dirty && !_dirty.Value)
            {
                throw new InvalidOperationException();
            }

            if (IsDefault.CurrentValue)
            {
                writer.WriteUnmanaged((byte)0);
            }
            else
            {
                writer.WriteUnmanaged((byte)1);
                if (mode == SyncMode.Full)
                {
                    foreach (var item in new ReadOnlySpan<ISyncSource>(_items))
                    {
                        item.Write(ref writer, mode);
                    }
                }
                else
                {
                    var dirtyItems = _dirtyItems;
                    writer.WriteUnmanaged(checked((byte)dirtyItems.Count));
                    foreach (var item in (ReadOnlySpan<KeyValuePair<byte, ISyncSource>>)CollectionsMarshal.AsSpan(dirtyItems))
                    {
                        writer.WriteUnmanaged(item.Key);
                        item.Value.Write(ref writer, mode);
                    }
                }
            }
        }

        public void ClearDirty()
        {
            if (_dirty.Value)
            {
                var dirtyItems = _dirtyItems;
                foreach (var item in (ReadOnlySpan<KeyValuePair<byte, ISyncSource>>)CollectionsMarshal.AsSpan(dirtyItems))
                {
                    item.Value.ClearDirty();
                }
                dirtyItems.Clear();
                _dirty.Value = false;
            }
        }

        public void Dispose()
        {
            _isDefault.Dispose();
            _dirty.Dispose();
            foreach (var item in new ReadOnlySpan<ISyncSource>(_items))
            {
                item.Dispose();
            }
        }
    }
}
