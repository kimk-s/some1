using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Utilities;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayHelpView : MonoBehaviour, IBackable
    {
        public Button faqButton;
        public Button aboutButton;
        public Button upButton;

        private IServiceProvider _serviceProvider;
        private GameObject _popups;

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _serviceProvider = serviceProvider;
            _popups = popups;
        }

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        private void Start()
        {
            var talksViewPrefab = new AsyncLazy<TalkGroupView>(
                () => ResourcesUtility.LoadViewAsync<TalkGroupView>(destroyCancellationToken));

            var aboutViewPrefab = new AsyncLazy<PlayAboutView>(
                () => ResourcesUtility.LoadViewAsync<PlayAboutView>(destroyCancellationToken));

            faqButton.OnClickAsObservable()
                .SubscribeAwait(
                    async (_, ct) => _popups.Add(await talksViewPrefab).Setup(_serviceProvider, _popups),
                    AwaitOperation.Drop);

            aboutButton.OnClickAsObservable()
                .SubscribeAwait(
                    async (_, ct) => _popups.Add(await aboutViewPrefab),
                    AwaitOperation.Drop);

            upButton.OnClickAsObservable().Subscribe(_ => Back());
        }
    }
}
