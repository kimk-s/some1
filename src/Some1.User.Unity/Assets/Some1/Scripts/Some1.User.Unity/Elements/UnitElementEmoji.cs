using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public class UnitElementEmoji : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public Vector2 SpriteRendererSize { get; private set; }

        private void Awake()
        {
            SpriteRendererSize = spriteRenderer.size;
        }
    }
}
