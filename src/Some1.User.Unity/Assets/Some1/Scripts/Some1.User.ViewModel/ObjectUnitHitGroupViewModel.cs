using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Some1.Play.Front;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectUnitHitGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ObjectUnitHitGroupViewModel(IObjectHitGroupFront front)
        {
            All = front.All.Select(x => new ObjectUnitHitViewModel(x).AddTo(_disposables)).ToArray();
            DamagePush = front.DamagePush.Connect().AddTo(_disposables);
            DamageMinimumCycles = front.DamageMinimumCycles.Connect().AddTo(_disposables);
        }

        public IReadOnlyList<ObjectUnitHitViewModel> All { get; }

        public ReadOnlyReactiveProperty<Vector2> DamagePush { get; }

        public ReadOnlyReactiveProperty<float?> DamageMinimumCycles { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
