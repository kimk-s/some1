using System;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public sealed class PlayTitleViewModel : IDisposable
    {
        private static int? _maxStar;
        private readonly CompositeDisposable _disposable = new();

        public PlayTitleViewModel(IPlayFront front)
        {
            MaxStar = front.Player.Title.MaxStar;

            StarCharacter = front.Player.Characters.Star.Connect().AddTo(_disposable);
            StarSpecialty = front.Player.Specialties.Star.Connect().AddTo(_disposable);
            StarGrade = front.Player.Title.StarGrade.Connect().AddTo(_disposable);

            PlayerId = front.Player.Title.PlayerId.Connect().AddTo(_disposable);

            Like = front.Player.Title.Like.Connect().AddTo(_disposable);
        }

        public static int MaxStar
        {
            get
            {
                if (_maxStar is null)
                {
                    throw new InvalidOperationException();
                }

                return _maxStar.Value;
            }

            private set => _maxStar = value;
        }

        public ReadOnlyReactiveProperty<Leveling> StarCharacter { get; }

        public ReadOnlyReactiveProperty<Leveling> StarSpecialty { get; }

        public ReadOnlyReactiveProperty<Leveling> StarGrade { get; }

        public ReadOnlyReactiveProperty<PlayerId> PlayerId { get; }

        public ReadOnlyReactiveProperty<Leveling> Like { get; }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
