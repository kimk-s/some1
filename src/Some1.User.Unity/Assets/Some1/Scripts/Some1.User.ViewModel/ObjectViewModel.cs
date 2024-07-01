using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Some1.Play.Front;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ObjectViewModel(IObjectFront front)
        {
            Active = front.Active.Connect().AddTo(_disposables);
            Position = front.Transform.Position.Connect().AddTo(_disposables);
            Unit = new ObjectUnitViewModel(front).AddTo(_disposables);
            Character = new ObjectCharacterViewModel(front).AddTo(_disposables);
            Buffs = front.Buffs.Select(x => new ObjectBuffViewModel(x, front.Shift, front.Transform).AddTo(_disposables)).ToArray();
        }

        public ReadOnlyReactiveProperty<bool> Active { get; }

        public ReadOnlyReactiveProperty<Vector2> Position { get; }

        public ObjectUnitViewModel Unit { get; }

        public ObjectCharacterViewModel Character { get; }

        public IReadOnlyList<ObjectBuffViewModel> Buffs { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        internal void Set(ObjectUnitCastViewModel? unitCast)
        {
            Unit.Set(unitCast);
        }
    }
}
