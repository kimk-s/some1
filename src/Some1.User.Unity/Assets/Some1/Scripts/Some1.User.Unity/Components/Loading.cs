using System;
using R3;
using R3.Triggers;
using UnityEngine;

namespace Some1.User.Unity.Components
{
    public class Loading : MonoBehaviour
    {
        public RectTransform rotating;

        private void OnEnable()
        {
            Observable.Return(Unit.Default)
                .Delay(TimeSpan.FromSeconds(0.5f))
                .TakeUntil(this.OnDisableAsObservable())
                .Subscribe(_ => rotating.gameObject.SetActive(true));
        }

        private void OnDisable()
        {
            rotating.gameObject.SetActive(false);
            rotating.rotation = Quaternion.identity;
        }

        private void Update()
        {
            if (rotating.gameObject.activeSelf)
            {
                rotating.Rotate(0, 0, 90 * Time.unscaledDeltaTime);
            }
        }
    }
}
