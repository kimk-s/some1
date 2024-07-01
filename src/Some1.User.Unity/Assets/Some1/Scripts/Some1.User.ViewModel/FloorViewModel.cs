using System;
using Some1.Play.Front;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class FloorViewModel : IDisposable
    {
        public FloorViewModel(IFloorFront front)
        {
            State = front.State;
        }

        public ReadOnlyReactiveProperty<FloorState?> State { get; }

        public void Dispose()
        {
        }
    }
}
