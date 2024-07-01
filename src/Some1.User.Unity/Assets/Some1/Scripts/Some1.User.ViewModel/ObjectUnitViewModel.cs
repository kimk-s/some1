using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public class ObjectUnitViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ObjectUnitViewModel(IObjectFront front)
        {
            CharacterType = front.Character.Info.Select(x => x?.Type).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Relation = front.Relation.Connect().ToReadOnlyReactiveProperty().AddTo(_disposables);

            ShiftHeight = front.Shift.Height;

            Battle = front.Battle.Battle.Select(x => x == true).ToReadOnlyReactiveProperty().AddTo(_disposables);

            Boosters = front.Boosters.Values.Select(x => new ObjectUnitBoosterViewModel(x).AddTo(_disposables)).ToArray();

            TakeStuffs = new ObjectUnitTakeStuffGroupViewModel(front.TakeStuffs).AddTo(_disposables);

            Hits = new ObjectUnitHitGroupViewModel(front.Hits).AddTo(_disposables);

            Position = front.Transform.Position;

            Emoji = new ObjectUnitEmojiViewModel(front.Emoji).AddTo(_disposables);

            Likes = front.Likes.Select(x => new ObjectUnitLikeViewModel(x).AddTo(_disposables)).ToArray();

            Title = new ObjectUnitTitleViewModel(front).AddTo(_disposables);

            Energies = new ObjectUnitEnergyGroupViewModel(front).AddTo(_disposables);

            Cast = new ReactiveProperty<ObjectUnitCastViewModel?>().AddTo(_disposables);

            Medal = new ObjectUnitMedalViewModel(front).AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<CharacterType?> CharacterType { get; }

        public ReadOnlyReactiveProperty<ObjectRelation?> Relation { get; }

        public ReadOnlyReactiveProperty<float> ShiftHeight { get; }

        public ReadOnlyReactiveProperty<bool> Battle { get; }

        public ReadOnlyReactiveProperty<Vector2> Position { get; }

        public IReadOnlyList<ObjectUnitBoosterViewModel> Boosters { get; }

        public ObjectUnitTakeStuffGroupViewModel TakeStuffs { get; }

        public ObjectUnitHitGroupViewModel Hits { get; }

        public ObjectUnitEmojiViewModel Emoji { get; }

        public IReadOnlyList<ObjectUnitLikeViewModel> Likes { get; }

        public ObjectUnitTitleViewModel Title { get; }

        public ObjectUnitEnergyGroupViewModel Energies { get; }

        public ReadOnlyReactiveProperty<ObjectUnitCastViewModel?> Cast { get; }

        public ObjectUnitMedalViewModel Medal { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        internal void Set(ObjectUnitCastViewModel? cast)
        {
            ((ReactiveProperty<ObjectUnitCastViewModel?>)Cast).Value = cast;
        }
    }
}
