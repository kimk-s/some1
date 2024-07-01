using System;
using System.Collections.Generic;
using System.Linq;
using Some1.Play.Front;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectUnitTakeStuffGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ObjectUnitTakeStuffGroupViewModel(IObjectTakeStuffGroupFront front)
        {
            All = front.All.Select(x => new ObjectUnitTakeStuffViewModel(x).AddTo(_disposables)).ToArray();
            ComboScore = front.ComboScore;
            ComboCycles = front.ComboCycles;
        }

        public IReadOnlyList<ObjectUnitTakeStuffViewModel> All { get; }

        public ReadOnlyReactiveProperty<int> ComboScore { get; }

        public ReadOnlyReactiveProperty<float> ComboCycles { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
