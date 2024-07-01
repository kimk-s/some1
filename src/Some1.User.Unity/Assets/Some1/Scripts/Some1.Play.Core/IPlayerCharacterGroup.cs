using System;
using System.Collections.Generic;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Core
{
    public interface IPlayerCharacterGroup
    {
        IReadOnlyDictionary<CharacterId, IPlayerCharacter> All { get; }
        ReadOnlyReactiveProperty<DateTime> PickTime { get; }
    }
}
