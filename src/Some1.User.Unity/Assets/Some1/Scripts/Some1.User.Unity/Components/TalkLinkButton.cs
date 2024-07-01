using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Info;
using Some1.User.Unity.Utilities;
using Some1.User.Unity.Views;
using Some1.User.ViewModel;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Components
{
    public class TalkLinkButton : MonoBehaviour
    {
        public TalkId id;
        public GameObject popups;
        public Button button;

        private IServiceProvider _serviceProvider;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _serviceProvider = serviceProvider;
            this.popups = popups;
        }

        private void Start()
        {
            var talkDetailViewPrefab = new AsyncLazy<TalkDetailView>(
                () => ResourcesUtility.LoadViewAsync<TalkDetailView>(destroyCancellationToken));

            button.OnClickAsObservable()
                .ThrottleFirst(TimeSpan.FromSeconds(0.1f))
                .SubscribeAwait(
                    async (_, ct) =>
                    {
                        var view = popups.Add(await talkDetailViewPrefab);
                        view.Setup(_serviceProvider.GetRequiredService<TalkGroupViewModel>().CreateDetail(id));
                    },
                    AwaitOperation.Drop)
                .AddTo(this);
        }
    }
}
