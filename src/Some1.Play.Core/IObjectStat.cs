using System;
using Some1.Play.Info;

namespace Some1.Play.Core
{
    public interface IObjectStat
    {
        StatId Id { get; }
        int Value { get; }

        event EventHandler<int>? ValueChanged;
    }
}
