using System.Collections.Generic;
using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public class PlayerElementEffectGroup : MonoBehaviour
    {
        private KeyValuePair<PlayerElementEffectId, PlayerElementEffect>[] _itemsById;

        public void Play(PlayerElementEffectId id)
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
                .OfComponent<PlayerElementEffect>()
                .Select(x =>
                    new KeyValuePair<PlayerElementEffectId, PlayerElementEffect>(
                        PlayerElementEffectIdHelper.Parse(x.name),
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

        private PlayerElementEffect? Get(PlayerElementEffectId id)
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
