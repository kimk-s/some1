using System;
using Some1.Play.Front;
using Some1.Play.Info;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class ObjectBuffViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public ObjectBuffViewModel(
            IObjectBuffFront front,
            IObjectShiftFront shiftFront,
            IObjectTransformFront transformFront)
        {
            Id = front.Id;
            Shift = shiftFront;
            Transform = transformFront;
        }

        public ReadOnlyReactiveProperty<BuffSkinId?> Id { get; }

        public IObjectShiftFront Shift { get; }

        public IObjectTransformFront Transform { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
