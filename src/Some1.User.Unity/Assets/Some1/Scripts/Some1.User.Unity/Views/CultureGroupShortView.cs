using System;
using Microsoft.Extensions.DependencyInjection;
using Some1.Prefs.UI;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class CultureGroupShortView : MonoBehaviour
    {
        public CultureShortView itemViewPrefab;
        public GameObject itemViewsContainer;

        private CultureGroupShortViewModel _viewModel;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<CultureGroupShortViewModel>();
        }

        private void Start()
        {
            foreach (var item in _viewModel.Items)
            {
                itemViewsContainer.Add(itemViewPrefab).Setup(item);
            }
        }
    }
}
