using System;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.ViewModel;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class FloorGroupView : MonoBehaviour
    {
        public FloorView floorViewPrefab;

        private FloorGroupViewModel _viewModel;
        private FloorView[] _floorViews;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Floors;
        }

        private void Start()
        {
            _floorViews = new FloorView[_viewModel.All.Count];
            int i = 0;
            foreach (var item in _viewModel.All)
            {
                var floorView = Instantiate(floorViewPrefab);
                _floorViews[i++] = floorView;
                floorView.Setup(item);
            }

            this.OnDestroyAsObservable().Subscribe(_ =>
            {
                foreach (var item in _floorViews)
                {
                    if (item && item.gameObject)
                    {
                        Destroy(item.gameObject);
                    }
                }
            });
        }
    }
}
