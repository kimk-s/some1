using System;
using Cysharp.Threading.Tasks;
using Some1.User.Unity.Utilities;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class PlayIndicatorsView : MonoBehaviour
    {
        private WalkIndicatorView _walkIndicatorView;
        private CastIndicatorView _castIndicatorView;
        private LikeIndicatorView _likeIndicatorView;

        private IServiceProvider _serviceProvider;

        public void Setup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private async UniTaskVoid Start()
        {
            _walkIndicatorView = Instantiate(await ResourcesUtility.LoadViewAsync<WalkIndicatorView>(destroyCancellationToken));
            _walkIndicatorView.Setup(_serviceProvider);
            this.OnDestroyAsObservable().Subscribe(_ =>
            {
                if (_walkIndicatorView)
                {
                    Destroy(_walkIndicatorView.gameObject);
                }
            });

            _castIndicatorView = Instantiate(await ResourcesUtility.LoadViewAsync<CastIndicatorView>(destroyCancellationToken));
            _castIndicatorView.Setup(_serviceProvider);
            this.OnDestroyAsObservable().Subscribe(_ =>
            {
                if (_castIndicatorView)
                {
                    Destroy(_castIndicatorView.gameObject);
                }
            });

            _likeIndicatorView = Instantiate(await ResourcesUtility.LoadViewAsync<LikeIndicatorView>(destroyCancellationToken));
            _likeIndicatorView.Setup(_serviceProvider);
            this.OnDestroyAsObservable().Subscribe(_ =>
            {
                if (_likeIndicatorView)
                {
                    Destroy(_likeIndicatorView.gameObject);
                }
            });
        }
    }
}
