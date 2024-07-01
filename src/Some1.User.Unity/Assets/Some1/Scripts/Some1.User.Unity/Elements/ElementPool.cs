using System.Collections.Generic;
using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public class ElementPool : MonoBehaviour
    {
        private readonly Dictionary<string, ElementSet> _sets = new();
        private float _t;

        public Element? Rent(string path)
        {
            if (!_sets.TryGetValue(path, out var set))
            {
                set = new(path);
                _sets[path] = set;
            }

            return set.Rent();
        }

        public void Return(string path, Element element)
        {
            _sets[path].Return(element);
        }

        private void Update()
        {
            _t += Time.deltaTime;
            if (_t > 30)
            {
                Swap();
                _t = 0;
            }
        }

        private void OnDestroy()
        {
            foreach (var item in _sets.Values)
            {
                item.Dispose();
            }
        }

        private void Swap()
        {
            foreach (var item in _sets.Values)
            {
                item.Swap();
            }
        }
    }
}
