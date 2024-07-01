using System;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitHitGroupView : MonoBehaviour
    {
        public ObjectUnitHitView[] items;

        public void Setup(ObjectUnitHitGroupViewModel viewModel, Func<UnitElementHitGroup?> getElement)
        {
            Debug.Assert(items.Length == viewModel.All.Count);

            for (int i = 0; i < items.Length; i++)
            {
                int index = i;

                items[i].Setup(
                    viewModel.All[i],
                    () =>
                    {
                        var element = getElement();
                        return element == null ? null : (element.items[index], element.damageColor, element.recoveryColor);
                    });
            }
        }

        public void Apply()
        {
            foreach (var item in items)
            {
                item.Apply();
            }
        }
    }
}
