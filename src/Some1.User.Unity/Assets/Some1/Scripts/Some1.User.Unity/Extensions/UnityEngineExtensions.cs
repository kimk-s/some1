namespace UnityEngine
{
    public static class UnityEngineExtensions
    {
        internal static T Popup<T>(this T component) where T : Component
        {
            component.transform.SetAsLastSibling();
            component.gameObject.SetActive(true);
            return component;
        }
    }
}
