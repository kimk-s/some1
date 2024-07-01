using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayPageView : MonoBehaviour
    {
        public PlayerFaceView faceView;
        public PlayerGameToastView gameToast;
        public PlayerGameView game;
        public PlayLocationView location;
        public PlayPageEmojiView emojis;
        public PlayerGameReadyView gameReady;
        public PlayJoysticksView joysticks;
        public Image menuCharacterIconImage;
        public Button menuButton;
        public Button gameDetailButton;
        public Button mapDetailButton;
        public Button gameMenuButton;
        public PlayPopupsView popups;

        private PlayPageViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            var viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Page;

            faceView.Setup(serviceProvider);
            gameToast.Setup(serviceProvider, popups);
            game.Setup(serviceProvider);
            location.Setup(serviceProvider);
            emojis.Setup(viewModel.Emoji);
            gameReady.Setup(serviceProvider);
            joysticks.Setup(serviceProvider);

            _viewModel = viewModel;
        }

        private void Start()
        {
            _viewModel.Active.SubscribeToActive(gameObject).AddTo(this);

            menuButton.BindToOnClick(_ => popups.OpenMenuAsync(destroyCancellationToken)).AddTo(this);

            gameDetailButton.BindToOnClick(_ => popups.OpenGameDetailAsync(destroyCancellationToken)).AddTo(this);

            mapDetailButton.BindToOnClick(_ => popups.OpenLocationDetailAsync(destroyCancellationToken)).AddTo(this);

            gameMenuButton.BindToOnClick(_ => popups.OpenGameMenuAsync(destroyCancellationToken)).AddTo(this);
        }
    }
}
