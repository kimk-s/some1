using System;

namespace Some1.Play.Core
{
    public interface IPlayerPremium
    {
        bool IsPremium { get; }
        DateTime EndTime { get; }
    }
}
