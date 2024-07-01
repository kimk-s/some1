using System;
using Some1.User.Unity.Utilities;
using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public class ElementManager : MonoBehaviour
    {
        public GameObject placeHolder;

        private Action _onAfterRent;
        private string? _path;
        [SerializeField]
        private Element? _element;

        public string? Path
        {
            get => _path;

            set
            {
                if (_path == value)
                {
                    return;
                }

                Return();
                _path = value;
                Rent();
            }
        }

        public Element? Element
        {
            get => _element;

            private set
            {
                if (_element == value)
                {
                    return;
                }

                _element = value;

                if (placeHolder != null)
                {
                    placeHolder.SetActive(value == null);
                }
            }
        }

        private static ElementPool Pool => GlobalBinding.Instance.ElementPool;

        public void Register(Action onAfterRent)
        {
            _onAfterRent = onAfterRent;
        }

        private void OnEnable()
        {
            Rent();
        }

#if UNITY_EDITOR
        private bool _isApplicationQuitting;
        private void OnApplicationQuit()
        {
            _isApplicationQuitting = true;
        }
#endif
        private void OnDisable()
        {
#if UNITY_EDITOR
            if (_isApplicationQuitting)
            {
                return;
            }
#endif
            Return();
        }

        private void Update()
        {
            Rent();
        }

        private void Rent()
        {
            if (Path == null || Element != null)
            {
                return;
            }

            Element = Pool.Rent(Path);

            if (Element == null)
            {
                return;
            }

            Element.gameObject.SetActive(true);
            _onAfterRent?.Invoke();
        }

        private void Return()
        {
            if (Path == null || Element == null)
            {
                Debug.Assert(Element == null);
                return;
            }

            Element.gameObject.SetActive(false);
            Pool.Return(Path, Element);
            Element = null;
        }
    }
}
