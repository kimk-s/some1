using System;

namespace Some1.Play.Info
{
    [Flags]
    public enum HierarchyTarget
    {
        None = 0,
        Self = 1,
        Root = 2,
        Parant = 4,
        Children = 8,
        Ancestors = 16,
        Descendants = 32,
        Siblings = 64,
    }
}
