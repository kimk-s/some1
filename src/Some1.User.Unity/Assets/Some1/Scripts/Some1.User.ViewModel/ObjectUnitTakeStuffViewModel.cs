using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectUnitTakeStuffViewModel : IDisposable
    {
        public ObjectUnitTakeStuffViewModel(IObjectTakeStuffFront front)
        {
            Stuff = front.Stuff;
            Cycles = front.Cycles;
        }

        public ReadOnlyReactiveProperty<Stuff?> Stuff { get; }

        public ReadOnlyReactiveProperty<float> Cycles { get; }

        public void Dispose()
        {
        }
    }
}
