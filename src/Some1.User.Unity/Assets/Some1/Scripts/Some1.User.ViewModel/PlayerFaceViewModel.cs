using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class PlayerFaceViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public PlayerFaceViewModel(IPlayFront front)
        {
            CharacterId = front.Player.Object.CharacterId.Connect().AddTo(_disposables);
            GameToastState = front.Player.GameToast.State.Connect().AddTo(_disposables);
            AttackFailNotAnyLoadCount = front.AttackFailNotAnyLoadCount.Connect().AddTo(_disposables);
            CastItems = front.Player.Object.Cast.Items.Values
                .Select(x => new PlayerFaceCastItemViewModel(x).AddTo(_disposables))
                .ToDictionary(x => x.Id);
            TakeStuffsComboScore = front.Player.Object.TakeStuffs.ComboScore.Connect().AddTo(_disposables);
            Like = front.Player.Title.Like.Connect().AddTo(_disposables);
        }

        public ReadOnlyReactiveProperty<CharacterId?> CharacterId { get; }

        public ReadOnlyReactiveProperty<PlayerGameToastState> GameToastState { get; }

        public ReadOnlyReactiveProperty<int> AttackFailNotAnyLoadCount { get; }

        public IReadOnlyDictionary<CastId, PlayerFaceCastItemViewModel> CastItems { get; }

        public ReadOnlyReactiveProperty<int> TakeStuffsComboScore { get; }

        public ReadOnlyReactiveProperty<Leveling> Like { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
