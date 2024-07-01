using System;
using Some1.Auth.Front;
using Some1.Wait.Back;

namespace Some1.Wait.Front.Internal
{
    internal sealed class WaitUserFront : IWaitUserFront
    {
        public WaitUserFront(WaitUserBack back, IAuthUserFront auth)
        {
            Id = back.Id;
            DisplayName = auth.DisplayName;
            Email = auth.Email;
            PhotoUrl = auth.PhotoUrl;
            PlayId = back.PlayId;
            IsPlaying = back.IsPlaying;
            Manager = back.Manager;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public string Email { get; }

        public Uri? PhotoUrl { get; }

        public string PlayId { get; }

        public bool IsPlaying { get; }

        public bool Manager { get; }
    }
}
