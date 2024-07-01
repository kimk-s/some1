using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Some1.Prefs.Front;
using Some1.UI;

namespace Some1.Prefs.UI
{
    public sealed class CultureGroupLongViewModel
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly Func<CultureGroupExecutingErrorViewModel> _createCultureGroupExecutingErrorViewModel;

        public CultureGroupLongViewModel(IPrefsFront front)
        {
            Destroy = front.Culture
                .Select(x => x != Culture.None)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            var executingState = new ReactiveProperty<TaskState>().AddTo(_disposables);
            ExecutingErrorViewOpen = executingState.Select(x => x.Exception is not null).ToReadOnlyReactiveProperty();

            var selectCulture = new ReactiveCommand<Culture>().AddTo(_disposables);
            selectCulture.SubscribeAwait(
                async (x, ct) => await front.SetCultureAsync(x, ct),
                AwaitOperation.Drop,
                executingState);

            Items = EnumForUnity.GetValues<Culture>()
                .Where(x => x != Culture.None)
                .Select(x => new CultureLongViewModel(x, selectCulture).AddTo(_disposables))
                .ToArray();

            IsExecutingActive = executingState.Select(x => x.IsRunning).ToReadOnlyReactiveProperty();

            Page = Destroy
                .Select(x => !x)
                .ToReadOnlyReactiveProperty()
                .AddTo(_disposables);

            _createCultureGroupExecutingErrorViewModel = () => new(executingState);
        }

        public ReadOnlyReactiveProperty<bool> Destroy { get; }

        public ReadOnlyReactiveProperty<bool> Page { get; }

        public IReadOnlyList<CultureLongViewModel> Items { get; }

        public ReadOnlyReactiveProperty<bool> IsExecutingActive { get; }

        public ReadOnlyReactiveProperty<bool> ExecutingErrorViewOpen { get; }

        public CultureGroupExecutingErrorViewModel CreateCultureGroupExecutingErrorViewModel() => _createCultureGroupExecutingErrorViewModel();

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
