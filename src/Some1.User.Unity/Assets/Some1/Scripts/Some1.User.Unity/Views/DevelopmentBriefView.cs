using System;
using Cysharp.Threading.Tasks;
using Some1.User.Unity.Utilities;
using R3;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class DevelopmentBriefView : MonoBehaviour, IBackable
    {
        public LogBriefView logView;
        public FpsBriefView fpsView;
        public GameObject popups;

        public Button openDetailButton;

        private IServiceProvider _services;
        private AsyncLazy<DevelopmentDetailView> _detailDevelopmentViewPrefab;

        public void Setup(IServiceProvider services)
        {
            _services = services;
            _detailDevelopmentViewPrefab = new(async () => await ResourcesUtility.LoadViewAsync<DevelopmentDetailView>(this.GetCancellationTokenOnDestroy()));
            logView.Setup(services);
            fpsView.Setup(services);
        }

        public bool Back() => BackUtility.Back(popups);

        private void Start()
        {
            openDetailButton.BindToOnClick(async _ => popups.AddSingle(await _detailDevelopmentViewPrefab).Setup(_services)).AddTo(this);
        }
    }
}
