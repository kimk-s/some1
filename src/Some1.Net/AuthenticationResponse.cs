using MemoryPack;

namespace Some1.Net
{
    [MemoryPackable]
    public partial class AuthenticationResponse
    {
        public AuthenticationResult Result { get; set; }
    }
}
