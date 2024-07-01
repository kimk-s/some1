using System;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class DevelopmentDetailView : MonoBehaviour, IBackable
    {
        public LogDetailView logView;
        public FpsDetailView fpsView;

        public Button closeButton;

        public void Setup(IServiceProvider services)
        {
            logView.Setup(services);
            fpsView.Setup(services);
            closeButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);
        }

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }
    }
}
