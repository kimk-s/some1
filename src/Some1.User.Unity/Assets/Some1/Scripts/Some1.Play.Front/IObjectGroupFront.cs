using System.Collections.Generic;
using ObservableCollections;

namespace Some1.Play.Front
{
    public interface IObjectGroupFront
    {
        IObservableCollection<IObjectFront> All { get; }
        IReadOnlyDictionary<int, IObjectFront> ActiveItems { get; }
    }
}
