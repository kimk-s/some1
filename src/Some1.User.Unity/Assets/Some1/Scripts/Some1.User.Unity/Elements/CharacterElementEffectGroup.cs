using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public class CharacterElementEffectGroup : MonoBehaviour
    {
        private KeyValuePair<CharacterElementEffectId, CharacterElementEffect>[] _itemsById;

        public void Play(CharacterElementEffectId id)
        {
            var item = Get(id);
            if (item != null)
            {
                item.Play();
            }
        }

        private void Awake()
        {
            _itemsById = gameObject.Children()
                .OfComponent<CharacterElementEffect>()
                .Select(x =>
                    new KeyValuePair<CharacterElementEffectId, CharacterElementEffect>(
                        CharacterElementEffectIdHelper.Parse(x.name),
                        x))
                .ToArray();
        }

        private void OnDisable()
        {
            foreach (var item in _itemsById)
            {
                item.Value.Stop();
            }
        }

        private CharacterElementEffect? Get(CharacterElementEffectId id)
        {
            foreach (var item in _itemsById)
            {
                if (item.Key == id)
                {
                    return item.Value;
                }
            }

            return null;
        }
    }
}
