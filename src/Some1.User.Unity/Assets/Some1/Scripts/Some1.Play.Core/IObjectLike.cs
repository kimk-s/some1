using System;

namespace Some1.Play.Core
{
    public interface IObjectLike
    {
        int Count { get; }

        event EventHandler<Like>? Added;
    }
}
