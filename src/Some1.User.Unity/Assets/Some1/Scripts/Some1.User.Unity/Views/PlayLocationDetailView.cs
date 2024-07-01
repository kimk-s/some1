using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayLocationDetailView : MonoBehaviour, IBackable
    {
        public GameObject popups;
        public ScrollRect scrollRect;
        public RectTransform regionsContainer;
        public GameObject regionsContent;
        public PlayRegionView regionViewPrefab;
        public RectTransform position;
        public TMP_Text regionText;
        public TMP_Text sectionText;
        public TMP_Text serverText;
        public TMP_Text channelText;
        public Button openExitPopupButton;
        public Button closeButton;
        public Button backButton;
        public Button focusPositionButton;
        public MessageListView messageList;
        public TopBarScroller topBarScroller;

        private PlayLocationDetailViewModel _viewModel;
        private AsyncLazy<PlayLocationDetailExitPopupView> _exitPopupViewPerfab;

        public bool Back()
        {
            if (gameObject.activeSelf)
            {
                if (messageList.Back())
                {
                    return true;
                }

                if (BackUtility.Back(popups))
                {
                    return true;
                }

                Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.LocationDetail;
            messageList.Setup(_viewModel.MessageList);
            _exitPopupViewPerfab = new(()
                => ResourcesUtility.LoadViewAsync<PlayLocationDetailExitPopupView>(destroyCancellationToken));
        }

        private void Start()
        {
            var rt = regionsContainer;
            LayoutRebuilderUtility.ForceRebuildBottomUpFromEnd(rt);
            float ratio = rt.rect.width / _viewModel.SpaceSize.Width;
            rt.sizeDelta = new(rt.sizeDelta.x, _viewModel.SpaceSize.Height * ratio);

            foreach (var item in _viewModel.Regions.Values.OrderBy(x => x.Id))
            {
                regionsContent.Add(regionViewPrefab).Setup(item, ratio);
            }

            _viewModel.Position
                .Select(x => x * ratio)
                .Subscribe(x => position.anchoredPosition = x.ToUnityVector2())
                .AddTo(this);

            _viewModel.RegionSection
                .Select(x => x.Id.Region.GetCode())
                .SubscribeToText(regionText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.RegionSection,
                    (_, x) => $"{PlaySectionIdConverter.GetCode(x.Id.Section)} ({R.GetString(x.Type.GetName())})")
                .SubscribeToText(sectionText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Server,
                    (_, server) => StringFormatter.FormatServerName(server.Region, server.City))
                .SubscribeToText(serverText)
                .AddTo(this);

            _viewModel.Server
                .Select(x => x.Number)
                .SubscribeToText(channelText)
                .AddTo(this);

            _viewModel.OpenExitPopup.BindTo(openExitPopupButton).AddTo(this);
            _viewModel.OpenExitPopup
                .Subscribe(async _ =>
                {
                    var error = _viewModel.GetExitError();

                    if (error != Play.Front.PlayFrontError.Success)
                    {
                        _viewModel.MessageList.Add.Execute(error);
                        return;
                    }

                    popups.AddSingle(await _exitPopupViewPerfab).Setup(_viewModel);
                })
                .AddTo(this);

            closeButton.OnClickAsObservable().Subscribe(_ => Close()).AddTo(this);

            backButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);

            focusPositionButton.OnClickAsObservable().Subscribe(_ => scrollRect.FocusOnItem(position)).AddTo(this);

            topBarScroller.scrollRect.normalizedPosition = Vector2.zero;
        }
    }
}
