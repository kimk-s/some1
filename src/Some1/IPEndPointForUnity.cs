using System.Net;

namespace Some1
{
    public static class IPEndPointForUnity
    {
        public static IPEndPoint Parse(string s)
        {
#if UNITY
            var splited = s.Split(':');
            if (splited.Length > 2)
            {
                throw new System.ArgumentOutOfRangeException(nameof(s));
            }
            return new(IPAddress.Parse(splited[0]), int.Parse(splited[1]));
#else
            return IPEndPoint.Parse(s);
#endif
        }
    }
}
