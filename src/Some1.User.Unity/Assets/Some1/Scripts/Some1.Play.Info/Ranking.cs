using MemoryPack;

namespace Some1.Play.Info
{
    [MemoryPackable]
    public readonly partial struct Ranking
    {
        public Ranking(byte number, int score, Title title)
        {
            Number = number;
            Score = score;
            Title = title;
        }

        public static Ranking Empty => new();

        public byte Number { get; }

        public int Score { get; }

        public Title Title { get; }

        public Medal Medal => GetMedal(Number);

        public static Medal GetMedal(byte number) => number switch
        {
            1 => Medal.Gold,
            2 => Medal.Silver,
            3 => Medal.Bronze,
            < 11 => Medal.Tulip,
            < 101 => Medal.Sunflower,
            _ => Medal.None,
        };
    }
}
