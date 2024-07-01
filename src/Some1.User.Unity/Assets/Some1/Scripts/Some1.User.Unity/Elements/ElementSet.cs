using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Some1.User.Unity.Utilities;

namespace Some1.User.Unity.Elements
{
    public class ElementSet : IDisposable
    {
        private readonly CancellationTokenSource _cts = new();
        private Stack<Element> _primaryElements = new();
        private Stack<Element> _secondaryElements = new();
        private UniTask<Element>? _original;

        public ElementSet(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public Element? Rent()
        {
            if (_primaryElements.TryPop(out var element)
                || _secondaryElements.TryPop(out element))
            {
                element.gameObject.SetActive(true);
                return element;
            }

            _original ??= ResourcesUtility.LoadAsync<Element>(Path, _cts.Token).Preserve();

            if (_original.Value.Status.IsCompleted())
            {
                var original = _original.Value.GetAwaiter().GetResult();
                if (original == null)
                {
                    throw new InvalidOperationException($"Failed to load Element from {Path}.");
                }

                element = UnityEngine.Object.Instantiate(original);
                return element;
            }

            return null;
        }

        public void Return(Element element)
        {
            _primaryElements.Push(element);
            element.gameObject.SetActive(false);
        }

        public void Swap()
        {
            while (_secondaryElements.TryPop(out var element))
            {
                UnityEngine.Object.Destroy(element.gameObject);
            }

            (_primaryElements, _secondaryElements) = (_secondaryElements, _primaryElements);
        }

        public void Dispose()
        {
            _cts.CancelSafely();
            _cts.Dispose();
        }
    }
}
