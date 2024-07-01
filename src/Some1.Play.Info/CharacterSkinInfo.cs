namespace Some1.Play.Info
{
    public sealed class CharacterSkinInfo
    {
        public CharacterSkinInfo(CharacterSkinId id)
        {
            Id = id;
        }

        public CharacterSkinId Id { get; }

        public bool IsPremium => Id.Skin != SkinId.Skin0;
    }
}
