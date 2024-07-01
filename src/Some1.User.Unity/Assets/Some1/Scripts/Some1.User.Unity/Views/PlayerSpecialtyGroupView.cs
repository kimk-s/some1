using System;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Play.Info;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using TMPro;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayerSpecialtyGroupView : MonoBehaviour
    {
        public TMP_Text starText;
        public PlayerSpecialtyView itemPrefab;
        public GameObject content;

        private IServiceProvider _serviceProvider;
        private PlayerSpecialtyGroupViewModel _viewModel;
        private GameObject _popups;
        private AsyncLazy<PlayerSpecialtyDetailView> _detailViewPrefab;
        private ReactiveCommand<SpecialtyId> _openDetail;

        public void Setup(IServiceProvider serviceProvider, GameObject popups)
        {
            _serviceProvider = serviceProvider;
            _viewModel = _serviceProvider.GetRequiredService<PlayViewModel>().Popups.Menu.Specialties;
            _popups = popups;
            _detailViewPrefab = new(() => ResourcesUtility.LoadViewAsync<PlayerSpecialtyDetailView>(this.GetCancellationTokenOnDestroy()));
            _openDetail = new ReactiveCommand<SpecialtyId>().AddTo(this);
            _openDetail.SubscribeAwait(
                async (x, ct) => _popups.AddSingle(await _detailViewPrefab).Setup(_viewModel.CreateDetail(x)),
                AwaitOperation.Drop);
        }

        private void Start()
        {
            _viewModel.Star.Select(x => $"{StringFormatter.FormatDetailPointWithStar(x)}")
                .SubscribeToText(starText)
                .AddTo(this);

            foreach (var item in _viewModel.Specialties.Values)
            {
                content.Add(itemPrefab).Setup(item, _openDetail);
            }
        }
    }
}
