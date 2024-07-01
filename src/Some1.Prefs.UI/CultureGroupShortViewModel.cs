using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Prefs.Front;

namespace Some1.Prefs.UI
{
    public sealed class CultureGroupShortViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();

        public CultureGroupShortViewModel(IPrefsFront front)
        {
            Items = EnumForUnity.GetValues<Culture>()
                .Where(x => x != Culture.None)
                .Select(x => new CultureShortViewModel(x, front).AddTo(_disposables))
                .ToArray();
        }

        public IReadOnlyList<CultureShortViewModel> Items { get; }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
