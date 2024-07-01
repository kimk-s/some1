using System;
using MemoryPack;

namespace Some1.Play.Info
{
    [MemoryPackable]
    public readonly partial struct GameResult
    {
        public GameResult(
            bool success,
            Game game,
            float cycles,
            DateTime endTime,
            GameResultCharacter character,
            GameResultSpecialty[] specialties)
        {
            Success = success;
            Game = game;
            Cycles = cycles;
            EndTime = endTime;
            Character = character;
            Specialties = specialties;
        }

        public bool Success { get; }

        public Game Game { get; }

        public float Cycles { get; }

        public DateTime EndTime { get; }

        public GameResultCharacter Character { get; }

        public GameResultSpecialty[] Specialties { get; }
    }
}
