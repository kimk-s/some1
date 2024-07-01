using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Unity.Linq
{
    internal static class LINQtoGameObjectExtensions
    {
        public static T AddSingle<T>(
            this GameObject parent,
            T childOriginal,
            TransformCloneType cloneType = TransformCloneType.KeepOriginal,
            bool? setActive = null,
            string specifiedName = null,
            bool setLayer = false,
            bool mayExists = false) where T : MonoBehaviour
        {
            var exists = parent.GetComponentInChildren<T>(true);
            if (exists)
            {
                if (!mayExists)
                {
                    //throw new System.InvalidOperationException($"{typeof(T).Name} type child is already exists.");
                    Debug.LogWarning($"{typeof(T).Name} type child is already exists.");
                }

                exists.gameObject.Destroy();
            }
            return parent.Add(childOriginal, cloneType, setActive, specifiedName, setLayer);
        }

        public static IEnumerable<T> OfComponent<T>(this GameObjectExtensions.ChildrenEnumerable enumerable) =>
            enumerable.Select(x => x.GetComponent<T>()).Where(x => x != null);
    }
}
