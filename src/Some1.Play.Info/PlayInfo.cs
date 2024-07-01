using System;

namespace Some1.Play.Info
{
    public sealed class PlayInfo
    {
        public PlayInfo(
            SpaceInfo space,
            NonPlayerGenerationInfoGroup nonPlayerGenerations,
            NonPlayerSpawningInfoGroup nonPlayerSpawnings,
            CharacterInfoGroup characters,
            CharacterAliveInfoGroup characterAlives,
            CharacterIdleInfoGroup characterIdles,
            CharacterCastInfoGroup characterCasts,
            CharacterCastStatInfoGroup characterCastStats,
            CharacterSkillInfoGroup characterSkills,
            CharacterSkillPropInfoGroup characterSkillProps,
            CharacterStatInfoGroup characterStats,
            CharacterEnergyInfoGroup characterEnergies,
            CharacterSkinInfoGroup characterSkins,
            CharacterSkinEmojiInfoGroup characterSkinEmojis,
            BuffInfoGroup buffs,
            BuffStatInfoGroup buffStats,
            BuffSkinInfoGroup buffSkins,
            BoosterInfoGroup boosters,
            SpecialtyInfoGroup specialties,
            RegionInfoGroup regions,
            RegionSectionInfoGroup regionSections,
            SeasonInfoGroup seasons,
            TriggerInfoGroup triggers,
            TalkInfoGroup talks)
        {
            Space = space ?? throw new ArgumentNullException(nameof(space));
            NonPlayerGenerations = nonPlayerGenerations ?? throw new ArgumentNullException(nameof(nonPlayerGenerations));
            NonPlayerSpawnings = nonPlayerSpawnings ?? throw new ArgumentNullException(nameof(nonPlayerSpawnings));
            Characters = characters ?? throw new ArgumentNullException(nameof(characters));
            CharacterAlives = characterAlives ?? throw new ArgumentNullException(nameof(characterAlives));
            CharacterIdles = characterIdles ?? throw new ArgumentNullException(nameof(characterIdles));
            CharacterCasts = characterCasts ?? throw new ArgumentNullException(nameof(characterCasts));
            CharacterCastStats = characterCastStats ?? throw new ArgumentNullException(nameof(characterCastStats));
            CharacterSkills = characterSkills ?? throw new ArgumentNullException(nameof(characterSkills));
            CharacterSkillProps = characterSkillProps ?? throw new ArgumentNullException(nameof(characterSkillProps));
            CharacterStats = characterStats ?? throw new ArgumentNullException(nameof(characterStats));
            CharacterEnergies = characterEnergies ?? throw new ArgumentNullException(nameof(characterEnergies));
            CharacterSkins = characterSkins ?? throw new ArgumentNullException(nameof(characterSkins));
            CharacterSkinEmojis = characterSkinEmojis ?? throw new ArgumentNullException(nameof(characterSkinEmojis));
            Buffs = buffs ?? throw new ArgumentNullException(nameof(buffs));
            BuffStats = buffStats ?? throw new ArgumentNullException(nameof(buffStats));
            BuffSkins = buffSkins ?? throw new ArgumentNullException(nameof(buffSkins));
            Boosters = boosters ?? throw new ArgumentNullException(nameof(boosters));
            Specialties = specialties ?? throw new ArgumentNullException(nameof(specialties));
            Regions = regions ?? throw new ArgumentNullException(nameof(regions));
            RegionSections = regionSections ?? throw new ArgumentNullException(nameof(regionSections));
            Seasons = seasons ?? throw new ArgumentNullException(nameof(seasons));
            Triggers = triggers ?? throw new ArgumentNullException(nameof(triggers));
            Talks = talks ?? throw new ArgumentNullException(nameof(talks));
        }

        public SpaceInfo Space { get; }

        public NonPlayerGenerationInfoGroup NonPlayerGenerations { get; }

        public NonPlayerSpawningInfoGroup NonPlayerSpawnings { get; }

        public CharacterInfoGroup Characters { get; }

        public CharacterAliveInfoGroup CharacterAlives { get; }

        public CharacterIdleInfoGroup CharacterIdles { get; }

        public CharacterCastInfoGroup CharacterCasts { get; }

        public CharacterCastStatInfoGroup CharacterCastStats { get; }

        public CharacterSkillInfoGroup CharacterSkills { get; }

        public CharacterSkillPropInfoGroup CharacterSkillProps { get; }

        public CharacterStatInfoGroup CharacterStats { get; }

        public CharacterEnergyInfoGroup CharacterEnergies { get; }

        public CharacterSkinInfoGroup CharacterSkins { get; }

        public CharacterSkinEmojiInfoGroup CharacterSkinEmojis { get; }

        public BuffInfoGroup Buffs { get; }

        public BuffStatInfoGroup BuffStats { get; }

        public BuffSkinInfoGroup BuffSkins { get; }

        public BoosterInfoGroup Boosters { get; }

        public SpecialtyInfoGroup Specialties { get; }

        public RegionInfoGroup Regions { get; }

        public RegionSectionInfoGroup RegionSections { get; }

        public SeasonInfoGroup Seasons { get; }

        public TriggerInfoGroup Triggers { get; }

        public TalkInfoGroup Talks { get; }
    }
}
