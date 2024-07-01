using Some1.User.ViewModel;

namespace Some1.User.Unity
{
    public static class JoystickHandledEventArgsExtensions
    {
        public static JoystickUiState ToJoystickUiState(this JoystickHandledEventArgs args)
            => new((JoystickUiStateType)(int)args.type, args.rotation, args.magnitude, args.isMagnitudeInClick);
    }
}
