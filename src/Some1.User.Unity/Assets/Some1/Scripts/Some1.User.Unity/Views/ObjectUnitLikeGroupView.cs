using System;
using System.Collections.Generic;
using Some1.User.Unity.Elements;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectUnitLikeGroupView : MonoBehaviour
    {
        public ObjectUnitLikeView[] items;

        public void Setup(IReadOnlyList<ObjectUnitLikeViewModel> viewModel, Func<UnitElementLikeGroup?> getElement)
        {
            Debug.Assert(items.Length == viewModel.Count);

            for (int i = 0; i < items.Length; i++)
            {
                int index = i;

                items[i].Setup(
                    viewModel[i],
                    () =>
                    {
                        var element = getElement();
                        return element == null ? null : element.items[index];
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
