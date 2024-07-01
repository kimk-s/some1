using System;
using Some1.Wait.Front;

namespace Some1.User.ViewModel
{
    public sealed class WaitUserViewModel
    {
        public WaitUserViewModel(IWaitUserFront front)
        {
            Id = front.Id;
            DisplayName = front.DisplayName;
            Email = front.Email;
            PhotoUrl = front.PhotoUrl;
            PlayId = front.PlayId;
            IsPlaying = front.IsPlaying;
            Manager = front.Manager;
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
