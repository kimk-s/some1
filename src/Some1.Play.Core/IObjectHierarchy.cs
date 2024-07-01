namespace Some1.Play.Core
{
    public interface IObjectHierarchy
    {
        IObject? Parent { get; }
        IObject Root { get; }
    }
}
