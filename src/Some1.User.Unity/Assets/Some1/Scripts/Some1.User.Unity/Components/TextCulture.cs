using System;
using R3;
using Some1.Resources;
using TMPro;
using UnityEngine;

namespace Some1.User.Unity.Components
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextCulture : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private SerializableReactiveProperty<string> _stringId;

        public string StringId
        {
            get => _stringId.Value;
            set => _stringId.Value = value;
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                _stringId.AddTo(this);
                _stringId.AsRStringObservable().Subscribe(x => GetText().text = x).AddTo(this);
            }
        }

        private TMP_Text GetText()
        {
            if (_text == null)
            {
                if (!TryGetComponent(out _text))
                {
                    throw new Exception($"Failed to get TMP_Text on ({name}, {_stringId?.Value})");
                }
            }

            return _text;
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
            if (_stringId is not null)
            {
                GetText().text = R.GetString(_stringId.Value);
            }
        }
#endif
    }
}
