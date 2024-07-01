using System;

namespace Some1.Play.Info
{
    public interface ICharacterAgentInfo
    {
    }

    public sealed class BattleCharacterAgentInfo : ICharacterAgentInfo
    {
        public BattleCharacterAgentInfo(
            float notFightWalkScanLength,
            float superProbability,
            float ultraProbability,
            float fightWalkScanLength = PlayConst.PlayerEyesightWidth * 0.7f)
        {
            if (notFightWalkScanLength < 0 || notFightWalkScanLength > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(notFightWalkScanLength));
            }

            if (superProbability < 0 || superProbability > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(superProbability));
            }

            if (ultraProbability < 0 || ultraProbability > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(ultraProbability));
            }

            if (fightWalkScanLength < 0 || fightWalkScanLength > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(fightWalkScanLength));
            }

            NotFightWalkScanLength = notFightWalkScanLength;
            SuperProbability = superProbability;
            UltraProbability = ultraProbability;
            FightWalkScanLength = fightWalkScanLength;
        }

        public float NotFightWalkScanLength { get; }

        public float SuperProbability { get; }

        public float UltraProbability { get; }

        public float FightWalkScanLength { get; }
    }
}
