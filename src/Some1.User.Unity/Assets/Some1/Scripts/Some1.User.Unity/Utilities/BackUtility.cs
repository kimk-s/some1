using System.Linq;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity.Utilities
{
    public static class BackUtility
    {
        public static bool Back(params GameObject[] gameObjects)
        {
            foreach (var item in gameObjects.SelectMany(x => x.Children().OfComponent<IBackable>()).Reverse())
            {
                if (item.Back())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
