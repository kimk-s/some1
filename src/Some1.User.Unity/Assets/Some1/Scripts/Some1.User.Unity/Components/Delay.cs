using UnityEngine;

namespace Some1.User.Unity.Components
{
    public class Delay : MonoBehaviour
    {
        public RectTransform mask;

        private float _normalizedValue;

        public float NormalizedValue
        {
            get => _normalizedValue;

            set
            {
                value = Mathf.Clamp01(value);

                if (_normalizedValue == value)
                {
                    return;
                }

                _normalizedValue = value;

                ApplyNormalizedValue();
            }
        }

        private void Start()
        {
            ApplyNormalizedValue();
        }

        private void ApplyNormalizedValue()
        {
            mask.anchorMin = new(mask.anchorMin.x, 1 - NormalizedValue);
        }
    }
}
