using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectCharacterViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ObjectCharacterViewModel(IObjectFront front)
        {
            Id = front.Character.Id
                .CombineLatest(front.SkinId, (character, skin)
                    => character is null || skin is null ? null : (CharacterSkinId?)new CharacterSkinId(character.Value, skin.Value))
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            Alive = front.Alive;
            Idle = front.Idle;
            Shift = front.Shift;
            Cast = front.Cast;
            Walk = front.Walk;
            GiveStuff = front.GiveStuff;
            Hits = front.Hits;
            Transform = front.Transform;
        }

        public ReadOnlyReactiveProperty<CharacterSkinId?> Id { get; }

        public IObjectAliveFront Alive { get; }

        public IObjectIdleFront Idle { get; }

        public IObjectShiftFront Shift { get; }

        public IObjectCastFront Cast { get; }

        public IObjectWalkFront Walk { get; }

        public IObjectGiveStuffFront GiveStuff { get; }

        public IObjectHitGroupFront Hits { get; }

        public IObjectTransformFront Transform { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
