using System;
using Some1.Play.Client.Tcp;
using Some1.Wait.Front;

namespace Some1.User.Unity
{
    public sealed class PlayAddressGettable : IPlayAddressGettable
    {
        private readonly IWaitFront _waitFront;

        public PlayAddressGettable(IWaitFront waitFront)
        {
            _waitFront = waitFront;
        }

        public string GetAddress()
        {
            return _waitFront.SelectedPlay.CurrentValue?.Address ?? throw new InvalidOperationException();
        }
    }
}
