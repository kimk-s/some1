using System;

namespace Some1.Play.Core
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
