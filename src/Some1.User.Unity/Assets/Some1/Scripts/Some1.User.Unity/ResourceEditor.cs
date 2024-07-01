using Some1.Resources;
using UnityEngine;

namespace Some1.User.Unity
{
    [ExecuteAlways]
    public class ResourceEditor : MonoBehaviour
    {
        public Culture culture;
        public Theme theme;

#if UNITY_EDITOR
        private void OnValidate()
        {
            R.SetCulture(culture);
            R.SetTheme(theme);
        }
#endif
    }
}
