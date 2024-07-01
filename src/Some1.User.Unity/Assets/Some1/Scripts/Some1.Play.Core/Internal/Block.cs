using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal class Block<TItemValue, TItemStatic> : IBlock<TItemValue>
        where TItemValue : IBlockItemValue<TItemStatic>
        where TItemStatic : IBlockItemStatic
    {
        private readonly List<BlockItem<TItemValue, TItemStatic>> _items = new();
        private readonly ParallelCollection<BlockMessage<TItemValue>> _messages;
        private readonly Control _control;

        internal Block(BlockId id, Area area, ParallelOptions parallelOptions, ITime time)
        {
            if (area.Type != AreaType.Rectangle)
            {
                throw new ArgumentOutOfRangeException(nameof(area));
            }

            Id = id;
            Area = area;
            _messages = new(parallelOptions);
            _control = new(time);
        }

        public BlockId Id { get; }

        public Area Area { get; }

        IEnumerable<TItemValue> IBlock<TItemValue>.Items => _items.Select(x => x.Value);

        public IControl Control => _control;

        internal List<BlockItem<TItemValue, TItemStatic>> Items => _items;

        internal IReadOnlyParallelCollection<BlockMessage<TItemValue>> Messages => _messages;

        internal void Add(TItemValue itemValue, ParallelToken? parallelToken)
        {
            if (itemValue is null)
            {
                throw new ArgumentNullException(nameof(itemValue));
            }

            if (parallelToken is null)
            {
                Add(itemValue);
            }
            else
            {
                if (!_messages.Add(new(true, itemValue), parallelToken))
                {
                    Console.WriteLine($"Failed to add message in Block.Add.");
                }
            }
        }

        internal void Remove(TItemValue itemValue, ParallelToken? parallelToken)
        {
            if (itemValue is null)
            {
                throw new ArgumentNullException(nameof(itemValue));
            }

            if (parallelToken is null)
            {
                Remove(itemValue);
            }
            else
            {
                if (!_messages.Add(new(false, itemValue), parallelToken))
                {
                    Console.WriteLine($"Failed to add message in Block.Remove.");
                }
            }
        }

        internal bool TryUpdate()
        {
            if (!_control.TryTake())
            {
                return false;
            }
            foreach (var item in _messages)
            {
                Execute(item);
            }
            _messages.Clear();
            return true;
        }

        private void Execute(BlockMessage<TItemValue> message)
        {
            if (message.isAdd)
            {
                Add(message.item);
            }
            else
            {
                Remove(message.item);
            }
        }

        protected virtual bool Add(TItemValue itemValue)
        {
            if (!itemValue.BlockIds.Contains(Id))
            {
                return false;
            }

            if (Contains(itemValue.Id))
            {
                return false;
            }

            _items.Add(new(itemValue));
            return true;
        }

        protected virtual bool Remove(TItemValue itemValue)
        {
            if (itemValue.BlockIds.Contains(Id))
            {
                return false;
            }

            var items = _items;
            int count = items.Count;
            for (int i = 0; i < count; i++)
            {
                if (items[i].Static.Id == itemValue.Id)
                {
                    items[i] = items[count - 1];
                    items.RemoveAt(count - 1);

                    return true;
                }
            }

            return false;
        }

        private bool Contains(int id)
        {
            foreach (var item in CollectionsMarshal.AsSpan(_items))
            {
                if (item.Static.Id == id)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
