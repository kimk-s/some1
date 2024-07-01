namespace Some1.Play.Info
{
    public readonly struct ObjectTargetInfo
    {
        public ObjectTargetInfo(CharacterTypeTarget character, TeamTargetInfo team)
        {
            Character = character;
            Team = team;
        }

        public static ObjectTargetInfo All { get; } = new(CharacterTypeTarget.All, TeamTargetInfo.All);

        public static ObjectTargetInfo None { get; } = new(CharacterTypeTarget.None, TeamTargetInfo.None);

        public CharacterTypeTarget Character { get; }

        public TeamTargetInfo Team { get; }
    }
}
