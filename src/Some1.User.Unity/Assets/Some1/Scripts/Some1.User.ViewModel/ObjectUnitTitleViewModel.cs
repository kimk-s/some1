using System;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public class ObjectUnitTitleViewModel : IDisposable
    {
        private readonly IDisposable _disposable;

        public ObjectUnitTitleViewModel(IObjectFront front)
        {
            StarGrade = front.Properties.Player
                .Select(x => x is null ? (Leveling?)null : new Leveling(x.Value.Title.Star, 1, PlayTitleViewModel.MaxStar, LevelingMethod.Plain))
                .ToReadOnlyReactiveProperty();

            PlayerId = front.Properties.Player
                .Select(x => x?.Title.PlayerId)
                .ToReadOnlyReactiveProperty();
            
            _disposable = Disposable.Combine(StarGrade, PlayerId);
        }

        public ReadOnlyReactiveProperty<Leveling?> StarGrade { get; }

        public ReadOnlyReactiveProperty<PlayerId?> PlayerId { get; }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
