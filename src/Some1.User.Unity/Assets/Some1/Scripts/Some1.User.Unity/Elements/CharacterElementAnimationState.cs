using System;
using UnityEngine;

namespace Some1.User.Unity.Elements
{
    public readonly struct CharacterElementAnimationState : IEquatable<CharacterElementAnimationState>
    {
        public CharacterElementAnimationState(CharacterElementAnimationStatus status, float length, float normalizedTime)
        {
            Status = status;
            Length = length;
            NormalizedTime = normalizedTime;
        }

        public CharacterElementAnimationStatus Status { get; }

        public float Length { get; }

        public float NormalizedTime { get; }

        public string Name => Status.GetString();

        public int HashName => Animator.StringToHash(Name);

        public float Time => Length * NormalizedTime;

        public override bool Equals(object obj) => obj is CharacterElementAnimationState other && Equals(other);

        public bool Equals(CharacterElementAnimationState other) => Status == other.Status && Length == other.Length && NormalizedTime == other.NormalizedTime;

        public override int GetHashCode() => HashCode.Combine(Status, Length, NormalizedTime);

        public override string ToString() => $"<{Status} {Length} {NormalizedTime}>";

        public static bool operator ==(CharacterElementAnimationState left, CharacterElementAnimationState right) => left.Equals(right);

        public static bool operator !=(CharacterElementAnimationState left, CharacterElementAnimationState right) => !(left == right);
    }
}
