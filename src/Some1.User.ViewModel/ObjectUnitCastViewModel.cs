using System;
using System.Collections.Generic;
using System.Linq;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectUnitCastViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ObjectUnitCastViewModel(IPlayerObjectCastFront castFront, IPlayerObjectTraitFront traitFront)
        {
            Items = castFront.Items.Values
                .Select(x => new ObjectUnitCastItemViewModel(x).AddTo(_disposables))
                .ToDictionary(x => x.Id);

            NextTrait = traitFront.Next.Connect().AddTo(_disposables);
        }

        public IReadOnlyDictionary<CastId, ObjectUnitCastItemViewModel> Items { get; }

        public ReadOnlyReactiveProperty<Trait> NextTrait { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
