using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using R3;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayPopups2View : MonoBehaviour
    {
        private PlayPopups2ViewModel _viewModel;
        private AsyncLazy<PipeErrorView> _pipeErrorViewPrefab;
        private AsyncLazy<PipeTerminatedView> _pipeTerminatedViewPrefab;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups2;
            _pipeErrorViewPrefab = new(() => ResourcesUtility.LoadViewAsync<PipeErrorView>(destroyCancellationToken));
            _pipeTerminatedViewPrefab = new(() => ResourcesUtility.LoadViewAsync<PipeTerminatedView>(destroyCancellationToken));
        }

        private void Start()
        {
            _viewModel.PipeError
                .Where(x => x is not null)
                .Subscribe(async _ => gameObject.Add(await _pipeErrorViewPrefab).Setup(_viewModel.GetPipeErrorViewModel()))
                .AddTo(this);

            _viewModel.PipeTerminated
                .Where(x => x)
                .Subscribe(async _ => gameObject.AddSingle(await _pipeTerminatedViewPrefab).Setup(_viewModel.GetPipeTerminatedViewModel()))
                .AddTo(this);
        }
    }
}
