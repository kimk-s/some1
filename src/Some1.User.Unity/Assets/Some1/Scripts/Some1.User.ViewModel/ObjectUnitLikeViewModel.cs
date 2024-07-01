using System;
using Some1.Play.Front;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectUnitLikeViewModel : IDisposable
    {
        public ObjectUnitLikeViewModel(IObjectLikeFront front)
        {
            Value = front.Value;
            Cycles = front.Cycles;
        }

        public ReadOnlyReactiveProperty<bool> Value { get; }

        public ReadOnlyReactiveProperty<float> Cycles { get; }

        public void Dispose()
        {
        }
    }
}
