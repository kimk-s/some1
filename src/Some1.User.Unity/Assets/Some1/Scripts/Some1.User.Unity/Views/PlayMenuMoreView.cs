using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Utilities;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayMenuMoreView : MonoBehaviour, IBackable
    {
        private enum MoreId
        {
            Seasons,
            User,
            Prefs,
            Helps,
        }

        public Button[] buttons;
        public Button blockerButton;

        private IServiceProvider _serviceProvider;
        private GameObject _popups;

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _serviceProvider = serviceProvider;
            _popups = popups;
        }

        public bool Back()
        {
            if (!gameObject.activeSelf)
            {
                return false;
            }
            gameObject.SetActive(false);
            return true;
        }

        private void Start()
        {
            var seasonsViewPrefab = new AsyncLazy<PlayerSeasonGroupView>(
                () => ResourcesUtility.LoadViewAsync<PlayerSeasonGroupView>(destroyCancellationToken));

            var userViewPrefab = new AsyncLazy<PlayUserView>(
                () => ResourcesUtility.LoadViewAsync<PlayUserView>(destroyCancellationToken));

            var prefsViewPrefab = new AsyncLazy<PrefsView>(
                () => ResourcesUtility.LoadViewAsync<PrefsView>(destroyCancellationToken));

            var helpViewPrefab = new AsyncLazy<PlayHelpView>(
                () => ResourcesUtility.LoadViewAsync<PlayHelpView>(destroyCancellationToken));

            var open = new ReactiveCommand<MoreId>().AddTo(this);
            open.SubscribeAwait(
                async (x, ct) =>
                {
                    switch (x)
                    {
                        case MoreId.Seasons:
                            _popups.AddSingle(await seasonsViewPrefab).Setup(_serviceProvider);
                            break;
                        case MoreId.User:
                            _popups.AddSingle(await userViewPrefab).Setup(_serviceProvider);
                            break;
                        case MoreId.Prefs:
                            _popups.AddSingle(await prefsViewPrefab).Setup(_serviceProvider);
                            break;
                        case MoreId.Helps:
                            _popups.AddSingle(await helpViewPrefab).Setup(_serviceProvider, _popups);
                            break;
                        default:
                            break;
                    }
                    Back();
                },
                AwaitOperation.Drop);

            foreach (var item in buttons.Select((x, n) => open.BindTo(x, _ => (MoreId)n)))
            {
                item.AddTo(this);
            }

            blockerButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);
        }
    }
}
