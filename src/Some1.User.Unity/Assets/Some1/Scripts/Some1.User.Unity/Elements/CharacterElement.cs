using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public class CharacterElement : Element
    {
        public Transform @base;
        public Vector2 size = new(1, 1);
        public new CharacterElementAnimation? animation;
        public CharacterElementEffectGroup? effects;
        public CharacterElementStuff? stuff;
    }
}
