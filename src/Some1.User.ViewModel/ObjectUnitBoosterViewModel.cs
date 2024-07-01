using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectUnitBoosterViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ObjectUnitBoosterViewModel(IObjectBoosterFront front)
        {
            Id = front.Id;
            Number = front.Number;
            Cycles = front.Cycles;
            NormalizedConsumingDelay = front.NormalizedConsumingDelay;
            Time = front.Time;
        }

        public BoosterId Id { get; }

        public ReadOnlyReactiveProperty<int> Number { get; }

        public ReadOnlyReactiveProperty<float> Cycles { get; }

        public ReadOnlyReactiveProperty<float> NormalizedConsumingDelay { get; }

        public ReadOnlyReactiveProperty<float> Time { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
