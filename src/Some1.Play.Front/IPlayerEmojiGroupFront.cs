using System.Collections.Generic;
using Some1.Play.Info;
using R3;

namespace Some1.Play.Front
{
    public interface IPlayerEmojiGroupFront
    {
        IReadOnlyDictionary<EmojiId, IPlayerEmojiFront> All { get; }
        ReadOnlyReactiveProperty<float> Delay { get; }
        ReadOnlyReactiveProperty<float> LikeDelay { get; }
        ReadOnlyReactiveProperty<float> NormalizedDelay { get; }
        ReadOnlyReactiveProperty<float> NormalizedLikeDelay { get; }
    }
}
