using System;
using R3;
using Some1.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Components
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Graphic))]
    public class GraphicTheme : MonoBehaviour
    {
        [SerializeField]
        private Graphic _graphic;
        [SerializeField]
        private SerializableReactiveProperty<ColorId> _colorId;

        public ColorId ColorId
        {
            get => _colorId.Value;
            set => _colorId.Value = value;
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                _colorId.AddTo(this);
                _colorId.AsRColorObservable().Subscribe(x => SetColor(x)).AddTo(this);
            }
        }

        private Graphic GetGraphic()
        {
            if (_graphic == null)
            {
                if (!TryGetComponent(out _graphic))
                {
                    throw new Exception($"Graphic is null on ({name}, {_colorId.Value})");
                }
            }

            return _graphic;
        }

        private void SetColor(System.Drawing.Color systemColor)
        {
            var graphic = GetGraphic();
            graphic.color = systemColor.ToUnityColor(graphic.color.a);
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            RefreshFromEditor();
        }

        private void Reset()
        {
            RefreshFromEditor();
        }

        private void OnValidate()
        {
            RefreshFromEditor();
        }

        private void RefreshFromEditor()
        {
            if (_colorId is not null)
            {
                SetColor(R.GetColor(_colorId.Value));
            }
        }
#endif
    }
}
