using TMPro;
using UnityEngine;

namespace Some1.User.Unity
{
    public static class TweenHelper
    {
        public static void TweenA(TMP_Text text, float cycles, Vector2 positionTo, float upTime = 0.2f, float downTime = 0.3f)
        {
            const float minScale = 0.5f;
            const float minAlpha = 0.5f;
            const float useAlpha = 1 - minAlpha;

            if (cycles < upTime)
            {
                float normalized = cycles / upTime;
                text.transform.localPosition = positionTo * normalized;
                text.transform.localScale = Vector3.one - new Vector3(minScale, minScale, 0) * (1 - normalized);
                SetAlpha(text, 1);
            }
            else if (cycles < downTime)
            {
                text.transform.localPosition = positionTo;
                SetAlpha(text, 1);
            }
            else
            {
                float normalized = (cycles - downTime) / (1 - downTime);
                text.transform.localPosition = positionTo * (1 - normalized);
                SetAlpha(text, minAlpha + useAlpha * (1 - normalized));
            }
        }

        public static void TweenA(SpriteRenderer spriteRenderer, float cycles, Vector2 positionTo, float upTime = 0.2f, float downTime = 0.4f)
        {
            const float minScale = 0.5f;
            const float minAlpha = 0.6f;
            const float useAlpha = 1 - minAlpha;

            if (cycles < upTime)
            {
                float normalized = cycles / upTime;
                spriteRenderer.transform.localPosition = positionTo * normalized;
                spriteRenderer.transform.localScale = Vector3.one - new Vector3(minScale, minScale, 0) * (1 - normalized);
                SetAlpha(spriteRenderer, 1);
            }
            else if (cycles < downTime)
            {
                spriteRenderer.transform.localPosition = positionTo;
                SetAlpha(spriteRenderer, 1);
            }
            else
            {
                float normalized = (cycles - downTime) / (1 - downTime);
                spriteRenderer.transform.localPosition = positionTo * (1 - normalized);
                SetAlpha(spriteRenderer, minAlpha + useAlpha * (1 - normalized));
            }
        }

        public static void TweenB(TMP_Text text, float cycles, Vector2 scaleTo, float upTime = 0.1f, float downEndTime = 0.2f, float hideTime = 0.8f)
        {
            if (cycles < upTime)
            {
                float normalized = cycles / upTime;
                text.transform.localScale = Vector3.one + (Vector3)((scaleTo - Vector2.one) * normalized);
                SetAlpha(text, 1);
            }
            else if (cycles < downEndTime)
            {
                float normalized = (cycles - upTime) / (downEndTime - upTime);
                text.transform.localScale = Vector3.one + (Vector3)((scaleTo - Vector2.one) * (1 - normalized));
                SetAlpha(text, 1);
            }
            else if (cycles < hideTime)
            {
                text.transform.localScale = Vector3.one;
                SetAlpha(text, 1);
            }
            else
            {
                float normalized = (cycles - hideTime) / (1 - hideTime);
                text.transform.localScale = Vector3.one;
                SetAlpha(text, 1 - normalized);
            }
        }

        public static void TweenB(Transform transform, float cycles, Vector2 scaleTo, float upTime = 0.1f, float downEndTime = 0.2f)
        {
            if (cycles < upTime)
            {
                float normalized = cycles / upTime;
                transform.localScale = Vector3.one + (Vector3)((scaleTo - Vector2.one) * normalized);
            }
            else if (cycles < downEndTime)
            {
                float normalized = (cycles - upTime) / (downEndTime - upTime);
                transform.localScale = Vector3.one + (Vector3)((scaleTo - Vector2.one) * (1 - normalized));
            }
            else
            {
                transform.transform.localScale = Vector3.one;
            }
        }

        private static void SetAlpha(TMP_Text text, float alpha)
        {
            if (text.color.a == alpha)
            {
                return;
            }

            var color = text.color;
            text.color = new(color.r, color.g, color.b, alpha);
        }

        private static void SetAlpha(SpriteRenderer spriteRenderer, float alpha)
        {
            if (spriteRenderer.color.a == alpha)
            {
                return;
            }

            var color = spriteRenderer.color;
            spriteRenderer.color = new(color.r, color.g, color.b, alpha);
        }
    }
}
