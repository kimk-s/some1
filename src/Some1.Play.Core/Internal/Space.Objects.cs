using System;
using System.Collections;
using System.Collections.Generic;
using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal partial class Space
    {
        internal SpaceObjects GetObjects(Area area, ParallelToken parallelToken) => GetObjects(area, ObjectTarget.All, parallelToken);

        internal SpaceObjects GetObjects(Area area, ObjectTarget target, ParallelToken parallelToken) => new(this, area, target, parallelToken);

        internal readonly struct SpaceObjects : IEnumerable<Object>
        {
            private readonly Space _space;
            private readonly Area _area;
            private readonly ObjectTarget _target;
            private readonly ParallelToken _parallelToken;

            internal SpaceObjects(Space space, Area area, ObjectTarget target, ParallelToken parallelToken)
            {
                _space = space;
                _area = area;
                _target = target;
                _parallelToken = parallelToken;
            }

            public Enumerator GetEnumerator() => new(_space, _area, _target, _parallelToken);

            IEnumerator<Object> IEnumerable<Object>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            internal struct Enumerator : IEnumerator<Object>, IEnumerator
            {
                private readonly Space _space;
                private readonly Area _area;
                private readonly ObjectTarget _target;
                private readonly ParallelToken _parallelToken;
                private BlockIdGroup.Enumerator _blockIdEnumerator;
                private List<BlockItem<Object, ObjectStatic>>.Enumerator _objectEnumerator;
                private bool _end;
                private Object? _current;

                internal Enumerator(Space space, Area area, ObjectTarget target, ParallelToken parallelToken)
                {
                    _space = space;
                    _area = area;
                    _target = target;
                    _parallelToken = parallelToken;
                    parallelToken.NewCode();
                    _blockIdEnumerator = space.GetBlockIds(area).GetEnumerator();
                    if (_blockIdEnumerator.MoveNext())
                    {
                        _objectEnumerator = _space.GetBlock(_blockIdEnumerator.Current).Items.GetEnumerator();
                        _end = false;
                    }
                    else
                    {
                        _objectEnumerator = default;
                        _end = true;
                    }
                    _current = null;
                }

                public readonly Object Current => _current!;

                object IEnumerator.Current => Current;

                public bool MoveNext()
                {
                    while (!_end)
                    {
                        while (_objectEnumerator.MoveNext())
                        {
                            var currentValue = _objectEnumerator.Current.Value;
                            if (currentValue.IsSpaceObject(_area, _target, _parallelToken))
                            {
                                _current = currentValue;
                                return true;
                            }
                        }

                        if (_blockIdEnumerator.MoveNext())
                        {
                            _objectEnumerator = _space.GetBlock(_blockIdEnumerator.Current).Items.GetEnumerator();
                        }
                        else
                        {
                            _objectEnumerator = default;
                            _end = true;
                        }
                    }

                    _current = null;
                    return false;
                }

                public void Reset()
                {
                    throw new NotSupportedException();
                }

                public readonly void Dispose()
                {
                }
            }
        }
    }
}
