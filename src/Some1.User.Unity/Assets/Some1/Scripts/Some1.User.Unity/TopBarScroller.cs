using System;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity
{
    public class TopBarScroller : MonoBehaviour
    {
        public RectTransform topBar;
        public ScrollRect scrollRect;

        private float _contentY;

        private void Start()
        {
            if (scrollRect.horizontal || !scrollRect.vertical)
            {
                throw new InvalidOperationException();
            }

            ScrollRectOnValueChanged(scrollRect.normalizedPosition);
            scrollRect.onValueChanged.AddListener(ScrollRectOnValueChanged);
        }

        private void ScrollRectOnValueChanged(Vector2 _)
        {
            float maxContentY = Mathf.Max(scrollRect.content.rect.size.y - scrollRect.viewport.rect.size.y, 0);
            float contentY = Mathf.Clamp(scrollRect.content.anchoredPosition.y, 0, maxContentY);
            float deltaConentY = contentY - _contentY;
            _contentY = contentY;

            topBar.anchoredPosition = new(
                topBar.anchoredPosition.x,
                Mathf.Clamp(topBar.anchoredPosition.y + deltaConentY, 0, topBar.rect.size.y));
        }
    }
}
