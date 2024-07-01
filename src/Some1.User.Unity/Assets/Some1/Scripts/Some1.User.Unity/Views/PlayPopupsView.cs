using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Some1.User.Unity.Utilities;
using R3;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayPopupsView : MonoBehaviour
    {
        private AsyncLazy<PlayMenuView> _menuView;
        private AsyncLazy<PlayerGameDetailView> _gameDetailView;
        private AsyncLazy<PlayLocationDetailView> _LocationDetailView;
        private AsyncLazy<PlayerEmojiGroupView> _emojisView;
        private AsyncLazy<PlayerGameMenuView> _gameMenuView;
        private bool _emojisViewLoaded;

        public void Setup(IServiceProvider serviceProvider)
        {
            _menuView = new(async () =>
            {
                var view = gameObject.AddSingle(await ResourcesUtility.LoadViewAsync<PlayMenuView>(destroyCancellationToken));
                view.Setup(serviceProvider);
                return view;
            });
            _gameDetailView = new(async () =>
            {
                var view = gameObject.AddSingle(await ResourcesUtility.LoadViewAsync<PlayerGameDetailView>(destroyCancellationToken));
                view.Setup(serviceProvider);
                return view;
            });
            _LocationDetailView = new(async () =>
            {
                var view = gameObject.AddSingle(await ResourcesUtility.LoadViewAsync<PlayLocationDetailView>(destroyCancellationToken));
                view.Setup(serviceProvider);
                return view;
            });
            _emojisView = new(async () =>
            {
                var view = gameObject.AddSingle(await ResourcesUtility.LoadViewAsync<PlayerEmojiGroupView>(destroyCancellationToken));
                view.Setup(serviceProvider);
                return view;
            });
            _gameMenuView = new(async () =>
            {
                var view = gameObject.AddSingle(await ResourcesUtility.LoadViewAsync<PlayerGameMenuView>(destroyCancellationToken));
                view.Setup(serviceProvider);
                return view;
            });
        }

        public async UniTask OpenMenuAsync(CancellationToken cancellationToken)
        {
            (await _menuView).Popup();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async UniTask OpenGameDetailAsync(CancellationToken cancellationToken)
        {
            (await _gameDetailView).Popup();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async UniTask OpenLocationDetailAsync(CancellationToken cancellationToken)
        {
            (await _LocationDetailView).Popup();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async UniTask OpenEmojisAsync(CancellationToken cancellationToken)
        {
            (await _emojisView).Popup();
            _emojisViewLoaded = true;
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async UniTask OpenOrCloseEmojisAsync(CancellationToken cancellationToken)
        {
            var emojisView = await _emojisView;
            cancellationToken.ThrowIfCancellationRequested();

            if (_emojisViewLoaded && emojisView.gameObject.activeSelf)
            {
                _emojisView.Task.GetAwaiter().GetResult().Back();
            }
            else
            {
                emojisView.Popup();
            }

            _emojisViewLoaded = true;
        }

        public async UniTask<PlayerGameMenuView> OpenGameMenuAsync(CancellationToken cancellationToken)
        {
            var view = await _gameMenuView;
            cancellationToken.ThrowIfCancellationRequested();
            view.Popup();
            return view;
        }
    }
}
