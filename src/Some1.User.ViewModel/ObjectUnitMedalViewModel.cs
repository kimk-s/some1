using System;
using System.Linq;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public class ObjectUnitMedalViewModel : IDisposable
    {
        public ObjectUnitMedalViewModel(IObjectFront front)
        {
            Value = front.Properties.Player.Select(x => x?.Title.Medal).ToReadOnlyReactiveProperty();
        }

        public ReadOnlyReactiveProperty<Medal?> Value { get; }

        public void Dispose()
        {
            ((IDisposable)Value).Dispose();
        }
    }
}
