using System;
using System.Collections.Generic;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerCharacterGroupFront
    {
        IReadOnlyDictionary<CharacterId, IPlayerCharacterFront> All { get; }
        ReadOnlyReactiveProperty<IPlayerCharacterFront?> Selected { get; }
        ReadOnlyReactiveProperty<DateTime> PickTime { get; }
        ReadOnlyReactiveProperty<TimeSpan> PickTimeRemained { get; }
        ReadOnlyReactiveProperty<Leveling> Star { get; }
    }
}
