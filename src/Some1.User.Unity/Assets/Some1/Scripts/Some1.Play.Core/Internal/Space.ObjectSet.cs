using System;
using System.Collections;
using System.Collections.Generic;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal sealed partial class Space
    {
        [Obsolete]
        private ObjectSet GetObjectSet(Area area) => GetObjectSet(area, static (_, __) => true, false);

        [Obsolete]
        internal void GetObjectSet(Area area, ObjectTarget objectTarget, ICollection<Object> results, int maxCount = 0)
        {
            using var objects = GetObjectSet(area, static (x, state) => x.IsTarget(state), objectTarget, maxCount);
            foreach (var item in objects)
            {
                results.Add(item);
            }
        }

        [Obsolete]
        private ObjectSet GetObjectSet<TState>(Area area, Func<Object, TState, bool> filter, TState state, int maxCount = 0)
        {
            if (maxCount < 1)
            {
                maxCount = int.MaxValue;
            }

            var objectSetState = s_objectSetState.Value!;
            objectSetState.Set(area, filter, state, maxCount, this);
            return new(objectSetState);
        }

        private readonly struct ObjectSet : IEnumerable<Object>, IDisposable
        {
            private readonly ObjectSetState _state;

            public ObjectSet(ObjectSetState state) => _state = state;

            public HashSet<Object>.Enumerator GetEnumerator() => _state.GetEnumerator();

            public void Dispose() => _state.Reset();

            IEnumerator<Object> IEnumerable<Object>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class ObjectSetState : IEnumerable<Object>
        {
            private readonly HashSet<Object> _objects = new();

            private bool _set;

            public int Count => _objects.Count;

            public void Set<TState>(Area area, Func<Object, TState, bool> filter, TState state, int maxCount, Space space)
            {
                if (_set)
                {
                    throw new InvalidOperationException();
                }

                foreach (var id in space.GetBlockIds(area))
                {
                    if (!Add(area, filter, state, maxCount, space, id))
                    {
                        break;
                    }
                }

                _set = true;
            }

            public HashSet<Object>.Enumerator GetEnumerator()
            {
                if (!_set)
                {
                    throw new InvalidOperationException();
                }

                return _objects.GetEnumerator();
            }

            public void Reset()
            {
                if (!_set)
                {
                    return;
                }

                _objects.Clear();
                _set = false;
            }

            IEnumerator<Object> IEnumerable<Object>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private bool Add<TState>(
                Area area,
                Func<Object, TState, bool> filter,
                TState state,
                int maxCount,
                Space space,
                BlockId blockId
                )
            {
                var block = space.GetBlock(blockId);

                if (!area.IntersectsWith(block.Area))
                {
                    return true;
                }

                foreach (var item in block.Items)
                {
                    if (area.IntersectsWith(item.Value.Properties.Area) && filter(item.Value, state))
                    {
                        if (!_objects.Add(item.Value))
                        {
                            continue;
                        }

                        if (_objects.Count >= maxCount)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
