using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class PlayerFaceCastItemViewModel : IDisposable
    {
        public PlayerFaceCastItemViewModel(IPlayerObjectCastItemFront front)
        {
            Id = front.Id;
            LoadCount = front.LoadCount.Connect();
            AnyLoadCount = front.AnyLoadCount.Connect();
        }

        public CastId Id { get; }

        public ReadOnlyReactiveProperty<int> LoadCount { get; }

        public ReadOnlyReactiveProperty<bool> AnyLoadCount { get; }

        public void Dispose()
        {
            ((IDisposable)LoadCount).Dispose();
            ((IDisposable)AnyLoadCount).Dispose();
        }
    }
}
