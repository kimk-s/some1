using System;

namespace Some1.User.ViewModel
{
    public enum JoystickUiStateType
    {
        Down,
        Drag,
        Up,
        Click,
        Cancel
    }

    public readonly struct JoystickUiState : IEquatable<JoystickUiState>
    {
        public JoystickUiState(JoystickUiStateType type, float rotation, float magnitude, bool isMagnitudeInClick)
        {
            Type = type;
            Rotation = rotation;
            Magnitude = magnitude;
            IsMagnitudeInClick = isMagnitudeInClick;
        }

        public JoystickUiStateType Type { get; }

        public float Rotation { get; }

        public float Magnitude { get; }

        public bool IsMagnitudeInClick { get; }

        public bool IsOn => Type == JoystickUiStateType.Down || Type == JoystickUiStateType.Drag;

        public override bool Equals(object? obj) => obj is JoystickUiState other && Equals(other);

        public bool Equals(JoystickUiState other) => Type == other.Type && Rotation == other.Rotation && Magnitude == other.Magnitude && IsMagnitudeInClick == other.IsMagnitudeInClick;

        public override int GetHashCode() => HashCode.Combine(Type, Rotation, Magnitude, IsMagnitudeInClick);

        public override string ToString() => $"<{Type} {Rotation} {Magnitude} {IsMagnitudeInClick}>";

        public static bool operator ==(JoystickUiState left, JoystickUiState right) => left.Equals(right);

        public static bool operator !=(JoystickUiState left, JoystickUiState right) => !(left == right);
    }
}
