using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ObservableCollections;
using R3;
using R3.Triggers;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectGroupView : MonoBehaviour
    {
        public ObjectView itemViewPrefab;

        private IServiceProvider _serviceProvider;
        private IObservableCollection<ObjectViewModel> _viewModel;
        private List<ObjectView> _itemViews = new();

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _viewModel = _serviceProvider.GetRequiredService<PlayViewModel>().Objects.All;

            Debug.Assert(_viewModel.Count == 0);

            _viewModel.ObserveAdd().Subscribe(x =>
            {
                var itemView = Instantiate(itemViewPrefab);
                itemView.Setup(x.Value);
                _itemViews.Insert(x.Index, itemView);
            }).AddTo(this);

            _viewModel.ObserveMove().Subscribe(x =>
            {
                var itemView = _itemViews[x.OldIndex];
                _itemViews.RemoveAt(x.OldIndex);
                _itemViews.Insert(x.NewIndex, itemView);
            }).AddTo(this);

            _viewModel.ObserveRemove().Subscribe(x =>
            {
                Destroy(_itemViews[x.Index].gameObject);
                _itemViews.RemoveAt(x.Index);
            }).AddTo(this);

            _viewModel.ObserveReplace().Subscribe(x =>
            {
                Destroy(_itemViews[x.Index].gameObject);
                var itemView = Instantiate(itemViewPrefab);
                itemView.Setup(x.NewValue);
                _itemViews[x.Index] = itemView;
            }).AddTo(this);

            _viewModel.ObserveReset().Subscribe(_ =>
            {
                foreach (var item in _itemViews)
                {
                    Destroy(item);
                }
                _itemViews.Clear();
            }).AddTo(this);

            this.OnDestroyAsObservable().Subscribe(_ =>
            {
                foreach (var item in _itemViews)
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
