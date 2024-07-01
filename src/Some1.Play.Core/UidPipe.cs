using Some1.Net;

namespace Some1.Play.Core
{
    public readonly struct UidPipe
    {
        public UidPipe(string uid, DuplexPipe pipe)
        {
            Uid = uid;
            Pipe = pipe;
        }

        public string Uid { get; }
        public DuplexPipe Pipe { get; }
    }
}
