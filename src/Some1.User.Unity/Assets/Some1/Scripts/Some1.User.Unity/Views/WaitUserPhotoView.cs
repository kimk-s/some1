using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.ViewModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class WaitUserPhotoView : MonoBehaviour
    {
        public GameObject placeHolder;
        public Image image;
        public GameObject manager;

        private static Dictionary<string, Task<Sprite?>> s_urlToSpriteTask = new();

        private ReadOnlyReactiveProperty<WaitUserViewModel?> _viewModel;

        public void Setup(ReadOnlyReactiveProperty<WaitUserViewModel?> viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            _viewModel.Select(x => x?.PhotoUrl)
                .ToReadOnlyReactiveProperty()
                .AddTo(this)
                .Subscribe(_ => LoadImageAsync().Forget());

            _viewModel.Select(x => x?.Manager ?? false).SubscribeToActive(manager).AddTo(this);
        }

        private async UniTask LoadImageAsync()
        {
            placeHolder.SetActive(true);
            image.gameObject.SetActive(false);

            string? url = _viewModel.CurrentValue?.PhotoUrl?.AbsoluteUri;
            if (url is null)
            {
                return;
            }

            if (!s_urlToSpriteTask.TryGetValue(url, out var spriteTask))
            {
                spriteTask = GetSpriteAsync(url);
                s_urlToSpriteTask[url] = spriteTask;
                if (s_urlToSpriteTask.Count > 10)
                {
                    Debug.LogWarning("Too many user photo sprites.");
                }
            }

            var sprite = await spriteTask;
            if (sprite == null)
            {
                s_urlToSpriteTask.Remove(url);
                return;
            }

            if (url != _viewModel.CurrentValue?.PhotoUrl?.AbsoluteUri)
            {
                return;
            }

            image.sprite = sprite;

            placeHolder.SetActive(false);
            image.gameObject.SetActive(true);
        }

        private async Task<Sprite?> GetSpriteAsync(string url)
        {
            try
            {
                using var www = UnityWebRequestTexture.GetTexture(url);
                await www.SendWebRequest().WithCancellation(destroyCancellationToken);

                var texture = DownloadHandlerTexture.GetContent(www);
                if (texture == null)
                {
                    return null;
                }

                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new(.5f, .5f));
            }
            catch (Exception ex)
            {
                Debug.LogWarning(@$"Failed to load user photo texture
{ex}");
                return null;
            }
        }
    }
}
