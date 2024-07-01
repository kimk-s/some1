using R3;

namespace Some1.UI
{
    public sealed class SharedCanExecute : ReactiveProperty<bool>
    {
        public SharedCanExecute() : base(true)
        {
        }
    }
}
