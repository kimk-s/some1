using System;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public readonly struct CastUiState : IEquatable<CastUiState>
    {
        public CastUiState(CastId id, JoystickUiState joystick)
        {
            Id = id;
            Joystick = joystick;
        }

        public CastId Id { get; }

        public JoystickUiState Joystick { get; }

        public override bool Equals(object? obj) => obj is CastUiState other && Equals(other);

        public bool Equals(CastUiState other) => Id == other.Id && Joystick.Equals(other.Joystick);

        public override int GetHashCode() => HashCode.Combine(Id, Joystick);

        public override string ToString() => $"<{Id} {Joystick}>";

        public static bool operator ==(CastUiState left, CastUiState right) => left.Equals(right);

        public static bool operator !=(CastUiState left, CastUiState right) => !(left == right);
    }
}
