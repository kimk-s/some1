using Some1.Play.Core.Paralleling;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal
{
    internal interface IObjectFactory
    {
        Object CreateRoot(bool leader);
        Object CreateChild(Object parent);
    }

    internal sealed class ObjectFactory : IObjectFactory
    {
        private readonly TriggerInfoGroup _triggerInfos;
        private readonly CharacterInfoGroup _characterInfos;
        private readonly CharacterAliveInfoGroup _characterAliveInfos;
        private readonly CharacterIdleInfoGroup _characterIdleInfos;
        private readonly CharacterCastInfoGroup _characterCastInfos;
        private readonly CharacterCastStatInfoGroup _characterCastStatInfos;
        private readonly CharacterStatInfoGroup _characterStatInfos;
        private readonly CharacterEnergyInfoGroup _characterEnergyInfos;
        private readonly CharacterSkinInfoGroup _characterSkinInfos;
        private readonly CharacterSkinEmojiInfoGroup _characterSkinEmojiInfos;
        private readonly BuffInfoGroup _buffInfos;
        private readonly BuffStatInfoGroup _buffStatInfos;
        private readonly BoosterInfoGroup _boosterInfos;
        private readonly SpecialtyInfoGroup _specialtyInfos;
        private readonly ParallelOptions _parallelOptions;
        private readonly RegionGroup _regions;
        private readonly Space _space;
        private readonly ITime _time;

        internal ObjectFactory(
            CharacterInfoGroup characterInfos,
            CharacterAliveInfoGroup characterAliveInfos,
            CharacterIdleInfoGroup characterIdleInfos,
            CharacterCastInfoGroup characterCastInfos,
            CharacterCastStatInfoGroup characterCastStatInfos,
            CharacterStatInfoGroup characterStatInfos,
            CharacterEnergyInfoGroup characterEnergyInfos,
            CharacterSkinInfoGroup characterSkinInfos,
            CharacterSkinEmojiInfoGroup characterSkinEmojiInfos,
            BuffInfoGroup buffInfos,
            BuffStatInfoGroup buffStatInfos,
            BoosterInfoGroup boosterInfos,
            SpecialtyInfoGroup specialtyInfos,
            TriggerInfoGroup triggerInfos,
            ParallelOptions parallelOptions,
            RegionGroup regions,
            Space space,
            ITime time)
        {
            _characterInfos = characterInfos;
            _characterAliveInfos = characterAliveInfos;
            _characterIdleInfos = characterIdleInfos;
            _characterCastInfos = characterCastInfos;
            _characterCastStatInfos = characterCastStatInfos;
            _characterStatInfos = characterStatInfos;
            _characterEnergyInfos = characterEnergyInfos;
            _characterSkinInfos = characterSkinInfos;
            _characterSkinEmojiInfos = characterSkinEmojiInfos;
            _buffInfos = buffInfos;
            _buffStatInfos = buffStatInfos;
            _boosterInfos = boosterInfos;
            _specialtyInfos = specialtyInfos;
            _triggerInfos = triggerInfos;
            _parallelOptions = parallelOptions;
            _regions = regions;
            _time = time;
            _space = space;
        }

        public Object CreateRoot(bool leader) => Create(leader, null);

        public Object CreateChild(Object parent) => Create(false, parent);

        private Object Create(bool leader, Object? parent) => new(
            leader,
            parent,
            _characterInfos,
            _characterAliveInfos,
            _characterIdleInfos,
            _characterCastInfos,
            _characterCastStatInfos,
            _characterStatInfos,
            _characterEnergyInfos,
            _characterSkinInfos,
            _characterSkinEmojiInfos,
            _buffInfos,
            _buffStatInfos,
            _boosterInfos,
            _specialtyInfos,
            _triggerInfos,
            _parallelOptions,
            this,
            _regions,
            _space,
            _time);
    }
}
