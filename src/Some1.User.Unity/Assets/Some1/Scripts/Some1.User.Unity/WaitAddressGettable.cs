using Microsoft.Extensions.Options;
using Some1.Wait.Back.MagicClient;

namespace Some1.User.Unity
{
    public sealed class WaitAddressGettableOptions
    {
        public string Address { get; set; } = null!;
    }

    public sealed class WaitAddressGettable : IWaitAddressGettable
    {
        private readonly string _address;

        public WaitAddressGettable(IOptions<WaitAddressGettableOptions> options)
        {
            _address = options.Value.Address;
        }

        public string GetAddress()
        {
            return _address;
        }
    }
}
