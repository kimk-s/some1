namespace Some1.Play.Core.Internal
{
    internal interface IBlockItemValue<TStatic>
        where TStatic : IBlockItemStatic
    {
        TStatic Static { get; }

        int Id { get; }
        BlockIdGroup BlockIds { get; }
    }

    internal interface IBlockItemStatic
    {
        int Id { get; }
    }
}
