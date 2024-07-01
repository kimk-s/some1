using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public enum UnitElementId
    {
        Mine,
        AllyPlayer,
        AllyNonPlayer,
        EnemyPlayer,
        EnemyNonPlayer,
    }

    public class UnitElement : Element
    {
        public Transform @base;
        public Transform top;
        public Transform bottom;
        public float titlePositionInNotBattle;
        public float titlePositionInBattle;

        public UnitElementBooster? booster;
        public UnitElementTakeStuffGroup takeStuffs;
        public UnitElementHitGroup hits;
        public UnitElementEmoji emoji;
        public UnitElementLikeGroup likes;
        public UnitElementTitle? title;
        public UnitElementEnergyGroup? energies;
        public UnitElementCast? cast;
        public UnitElementMedal? medal;
    }
}
