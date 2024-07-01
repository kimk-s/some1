using System;
using Some1.Resources;
using R3;
using UnityEngine;

namespace Some1.User.Unity.Components
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteRendererTheme : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
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

        private SpriteRenderer GetSpriteRenderer()
        {
            if (_spriteRenderer == null)
            {
                if (!TryGetComponent(out _spriteRenderer))
                {
                    throw new Exception($"Graphic is null on ({name}, {_colorId.Value})");
                }
            }

            return _spriteRenderer;
        }

        private void SetColor(System.Drawing.Color systemColor)
        {
            var spriteRenderer = GetSpriteRenderer();
            spriteRenderer.color = systemColor.ToUnityColor(spriteRenderer.color.a);
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
