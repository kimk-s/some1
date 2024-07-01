namespace Some1.Play.Info.TestHelpers;

public class TestPlayInfoOptions
{
    public IEnumerable<BuffInfo>? BuffInfos { get; set; }
}

public static class TestPlayInfoFactory
{
    public static PlayInfo Create() => Create(new());

    public static PlayInfo Create(TestPlayInfoOptions options)
        => new(
            new(
                new(20, 40),
                4,
                new(20, 20),
                20,
                2,
                2,
                4,
                8,
                4,
                8),
            new(Enumerable.Empty<NonPlayerGenerationInfo>()),
            new(Enumerable.Empty<NonPlayerSpawningInfo>()),
            new(new CharacterInfo[]
            {
                new(
                    CharacterId.Player1,
                    CharacterType.Player,
                    new(AreaType.Rectangle, 1),
                    move: new(BumpLevel.Low),
                    shift: new(1, 1, 1, 1),
                    walk: new(1, 1)),
                new(
                    CharacterId.WarpIn1,
                    CharacterType.Effect,
                    new(AreaType.Circle, 1)),
            }),
            new(Enumerable.Empty<CharacterAliveInfo>()),
            new(Enumerable.Empty<CharacterIdleInfo>()),
            new(new CharacterCastInfo[]
            {
                new(new(CharacterId.Player1, CastId.Attack), 2f, 1, 1, true, new(true, 0, 0), default)
            }),
            new(Enumerable.Empty<CharacterCastStatInfo>()),
            new(Enumerable.Empty<CharacterSkillInfo>()),
            new(Enumerable.Empty<CharacterSkillPropInfo>()),
            new(new CharacterStatInfo[]
            {
                new(new(CharacterId.Player1, StatId.Accel), 1),
                new(new(CharacterId.Player1, StatId.Offense), 1),
                new(new(CharacterId.Player1, StatId.Defense), 1),
                new(new(CharacterId.Player1, StatId.Health), 1),
                new(new(CharacterId.Player1, StatId.Mana), 1),
            }),
            new(new CharacterEnergyInfo[]
            {
                new(new(CharacterId.Player1, EnergyId.Health), 10),
                new(new(CharacterId.Player1, EnergyId.Mana), 10),
                new(new(CharacterId.WarpIn1, EnergyId.Health), 1),
            }),
            new(new CharacterSkinInfo[]
            {
                new(new(CharacterId.Player1, SkinId.Skin0))
            }),
            new(Enumerable.Empty<CharacterSkinEmojiInfo>()),
            new(options.BuffInfos ?? Enumerable.Empty<BuffInfo>()),
            new(Enumerable.Empty<BuffStatInfo>()),
            new(Enumerable.Empty<BuffSkinInfo>()),
            new(new BoosterInfo[]
            {
                new(BoosterId.Power, 1f),
                new(BoosterId.Accel, 1f),
            }),
            new(new SpecialtyInfo[]
            {
                new(SpecialtyId.Mob1, SeasonId.Season1, RegionId.Region1),
            }),
            new(new RegionInfo[]
            {
                new(RegionId.Region1, SeasonId.Season1),
            }),
            new(new RegionSectionInfo[]
            {
                new(new(RegionId.Region1, 0), SectionType.Town, SectionKind.None),
            }),
            new(new SeasonInfo[]
            {
                new(SeasonId.Season1, SeasonType.Current),
                new(SeasonId.Season2, SeasonType.ComingSoon),
            }),
            new(Enumerable.Empty<TriggerInfo>()),
            new(Enumerable.Empty<TalkInfo>()));
}
