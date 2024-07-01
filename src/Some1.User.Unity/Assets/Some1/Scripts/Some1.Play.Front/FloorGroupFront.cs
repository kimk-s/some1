using System;
using System.Collections.Generic;
using System.Drawing;
using R3;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    public sealed class FloorGroupFront : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly FloorFront[] _all;
        private readonly Dictionary<Point, FloorFront> _actives = new();
        private readonly Stack<FloorFront> _inactives;
        private readonly List<FloorFront> _toInactives = new();

        public FloorGroupFront(IRegionGroupFront regions, IPlayerFront player, ISpaceFront space, SpaceInfo info)
        {
            int count = (1 + (int)MathF.Ceiling(PlayConst.PlayerEyesightSize.Width / info.FloorTiles))
                * (1 + (int)MathF.Ceiling(PlayConst.PlayerEyesightSize.Height / info.FloorTiles));
            _all = new FloorFront[count];
            foreach (ref var item in _all.AsSpan())
            {
                item = new FloorFront(regions, info).AddTo(_disposables);
            }

            _inactives = new(_all);

            var eyesight = player.Object.Transform.Position
                .Select(x =>
                {
                    var result = Area.Rectangle(x, PlayConst.PlayerEyesightSize);
                    result.Intersect(space.Area);
                    return result;
                });
            var floorIds = eyesight
                .Select(x => Rectangle.FromLTRB(
                    (int)MathF.Floor(x.Left / info.FloorTiles),
                    (int)MathF.Floor(x.Top / info.FloorTiles),
                    (int)MathF.Ceiling(x.Right / info.FloorTiles),
                    (int)MathF.Ceiling(x.Bottom / info.FloorTiles)))
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            floorIds.Subscribe(ids =>
            {
                foreach (var item in _actives.Values)
                {
                    if (!ids.Contains(item.Id.Value!.Value))
                    {
                        _toInactives.Add(item);
                    }
                }

                foreach (var item in _toInactives)
                {
                    _actives.Remove(item.Id.Value!.Value);
                    item.Id.Value = null;
                    _inactives.Push(item);
                }
                _toInactives.Clear();

                for (int y = ids.Top; y < ids.Bottom; y++)
                {
                    for (int x = ids.Left; x < ids.Right; x++)
                    {
                        var id = new Point(x, y);

                        if (!_actives.ContainsKey(id))
                        {
                            var item = _inactives.Pop();
                            item.Id.Value = id;
                            _actives.Add(id, item);
                        }
                    }
                }
            });
        }

        public IReadOnlyList<IFloorFront> All => _all;

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
