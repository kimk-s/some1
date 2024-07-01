using R3;
using R3.Triggers;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using UnityEngine;

namespace Some1.User.Unity.Components
{
    public class RectLayoutRebuilder : MonoBehaviour
    {
        private void Start()
        {
            this.OnRectTransformDimensionsChangeAsObservable()
                .Merge(R.Culture.AsUnitObservable())
                .DebounceFrame(1)
                .SubscribeOn(UnityTimeProvider.PostLateUpdate)
                .Subscribe(_ => LayoutRebuilderUtility.ForceRebuildBottomUpFromRoot(gameObject))
                .AddTo(this);
        }
    }
}
