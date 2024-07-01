using System;
using System.Drawing;
using System.Numerics;
using R3;
using Some1.Play.Info;

namespace Some1.Play.Front
{
    public sealed class FloorFront : IFloorFront, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public FloorFront(IRegionGroupFront regions, SpaceInfo info)
        {
            Id = new ReactiveProperty<Point?>().AddTo(_disposables);

            State = Id
                .Select(x =>
                {
                    if (x is null)
                    {
                        return (FloorState?)null;
                    }
                    else
                    {
                        var area = Area.Rectangle(
                            new PointF(x.Value.X * info.FloorTiles, x.Value.Y * info.FloorTiles),
                            info.FloorTiles);

                        var section = regions.Get(area.Position);
                        if (section is null)
                        {
                            return null;
                        }

                        var delta = Vector2.Abs(area.Position - section.Area.Position);
                        FloorRole role;
                        if (delta.X < (info.PlazaTiles * 0.5f) && delta.Y < (info.PlazaTiles * 0.5f))
                        {
                            role = FloorRole.Plaza;
                        }
                        else if (delta.X < (info.RoadTiles * 0.5f) || delta.Y < (info.RoadTiles * 0.5f))
                        {
                            role = FloorRole.Road;
                        }
                        else
                        {
                            role = FloorRole.Plain;
                        }

                        int variation = x.GetHashCode() % FloorType.VariationCount;

                        return new(x.Value, area, new(section.Id.Region, section.Kind, role, variation));
                    }
                })
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<FloorState?> State { get; }

        internal ReactiveProperty<Point?> Id { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
