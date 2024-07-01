namespace Some1.Play.Info.Alpha
{
    public static class AlphaPlayInfoFactory
    {
        public static PlayInfo Create(AlphaPlayInfoEnvironment environment) => new(
            AlphaSpaceInfoFactory.Create(environment),
            new(AlphaNonPlayerGenerationInfosFactory.Create(environment)),
            new(AlphaNonPlayerSpawningInfosFactory.Create()),
            new(AlphaCharacterInfosFactory.Create()),
            new(AlphaCharacterAliveInfosFactory.Create()),
            new(AlphaCharacterIdleInfosFactory.Create()),
            new(AlphaCharacterCastInfosFactory.Create()),
            new(AlphaCharacterCastStatInfosFactory.Create()),
            new(AlphaCharacterSkillInfosFactory.Create()),
            new(AlphaCharacterSkillPropInfosFactory.Create()),
            new(AlphaCharacterStatInfosFactory.Create()),
            new(AlphaCharacterEnergyInfosFactory.Create(environment)),
            new(AlphaCharacterSkinInfosFactory.Create()),
            new(AlphaCharacterSkinEmojiInfosFactory.Create()),
            new(AlphaBuffInfosFactory.Create()),
            new(AlphaBuffStatInfosFactory.Create()),
            new(AlphaBuffSkinInfosFactory.Create()),
            new(AlphaBoosterInfoFactory.Create()),
            new(AlphaSpecialtyInfoFactory.Create()),
            new(AlphaRegionInfosFactory.Create()),
            new(AlphaRegionSectionInfosFactory.Create()),
            new(AlphaSeasonInfoFactory.Create()),
            new(AlphaTriggerInfosFactory.Create()),
            new(AlphaTalkInfosFactory.Create()));
    }
}
