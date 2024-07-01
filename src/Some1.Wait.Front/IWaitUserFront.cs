using System;

namespace Some1.Wait.Front
{
    public interface IWaitUserFront
    {
        string Id { get; }
        string DisplayName { get; }
        string Email { get; }
        Uri? PhotoUrl { get; }
        string PlayId { get; }
        bool IsPlaying { get; }
        bool Manager { get; }
    }
}
