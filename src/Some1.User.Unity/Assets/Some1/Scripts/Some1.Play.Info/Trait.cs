using System;

namespace Some1.Play.Info
{
    [Flags]
    public enum Trait : byte
    {
        None = 0,
        Common = 1,
        One = 2,
        Two = 4,
        All = Common | One | Two,
    }
}
