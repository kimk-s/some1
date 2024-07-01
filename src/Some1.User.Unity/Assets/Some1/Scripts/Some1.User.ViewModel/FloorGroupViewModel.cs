using System;
using System.Collections.Generic;
using System.Linq;
using Some1.Play.Front;
using R3;

namespace Some1.User.ViewModel
{
    public sealed class FloorGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposable = new();

        public FloorGroupViewModel(IReadOnlyList<IFloorFront> fronts)
        {
            All = fronts.Select(x => new FloorViewModel(x).AddTo(_disposable)).ToArray();
        }

        public IReadOnlyList<FloorViewModel> All { get; }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
