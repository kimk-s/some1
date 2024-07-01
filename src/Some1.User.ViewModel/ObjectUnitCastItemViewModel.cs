using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectUnitCastItemViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ObjectUnitCastItemViewModel(IPlayerObjectCastItemFront front)
        {
            Id = front.Id;
            NormalizedLoadWave = front.NormalizedLoadWave.Connect().AddTo(_disposables);
            LoadCount = front.LoadCount.Connect().AddTo(_disposables);
            MaxLoadCount = front.MaxLoadCount.Connect().AddTo(_disposables);
            AnyLoadCount = front.AnyLoadCount.Connect().AddTo(_disposables);
        }

        public CastId Id { get; }

        public ReadOnlyReactiveProperty<float> NormalizedLoadWave { get; }

        public ReadOnlyReactiveProperty<int> LoadCount { get; }

        public ReadOnlyReactiveProperty<int> MaxLoadCount { get; }

        public ReadOnlyReactiveProperty<bool> AnyLoadCount { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
