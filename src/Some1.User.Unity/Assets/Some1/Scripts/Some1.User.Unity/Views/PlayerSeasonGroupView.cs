using System;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.User.ViewModel;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerSeasonGroupView : MonoBehaviour, IBackable
    {
        public GameObject itemsContent;
        public PlayerSeasonView itemViewPrefab;
        public Button upButton;

        private IServiceProvider _serviceProvider;
        private PlayerSeasonGroupViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModel = _serviceProvider.GetRequiredService<PlayViewModel>().Popups.Menu.Seasons;
        }

        public bool Back()
        {
            Destroy(gameObject);
            return true; 
        }

        private void Start()
        {
            foreach (var item in _viewModel.Seasons.Values)
            {
                itemsContent.Add(itemViewPrefab).Setup(item);
            }

            upButton.OnClickAsObservable().Subscribe(_ => Back());
        }
    }
}
