using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Utilities
{
    public static class LayoutRebuilderUtility
    {
        public static void ForceRebuildBottomUpFromEnd(GameObject end) => ForceRebuildBottomUpFromEnd((RectTransform)end.transform);

        public static void ForceRebuildBottomUpFromEnd(RectTransform end)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(end);
            if (end.parent is RectTransform parent)
            {
                ForceRebuildBottomUpFromEnd(parent);
            }
        }

        public static void ForceRebuildBottomUpFromRoot(GameObject root)
        {
            if (!root.TryGetComponent<ILayoutController>(out _))
            {
                return;
            }

            foreach (var item in root.Children().OfComponent<ILayoutController>())
            {
                if (item is Component component)
                {
                    ForceRebuildBottomUpFromRoot(component.gameObject);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)root.transform);
        }
    }
}
