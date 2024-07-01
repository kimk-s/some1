using System;

namespace Some1.Play.Core
{
    public sealed class DateTimeClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
