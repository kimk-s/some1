using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectUnitHitViewModel : IDisposable
    {
        public ObjectUnitHitViewModel(IObjectHitFront front)
        {
            Hit = front.Hit;
            Cycles = front.Cycles;
            ToMe = front.ToMe;
            FromMe = front.FromMe;
        }

        public ReadOnlyReactiveProperty<HitPacket?> Hit { get; }

        public ReadOnlyReactiveProperty<float> Cycles { get; }

        public ReadOnlyReactiveProperty<bool> ToMe { get; }

        public ReadOnlyReactiveProperty<bool> FromMe { get; }

        public void Dispose()
        {
        }
    }
}
