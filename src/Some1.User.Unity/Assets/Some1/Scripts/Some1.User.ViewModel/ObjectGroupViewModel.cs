using System;
using System.Diagnostics;
using ObservableCollections;
using R3;
using Some1.Play.Front;

namespace Some1.User.ViewModel
{
    public sealed class ObjectGroupViewModel : IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly ObservableList<ObjectViewModel> _all;
        private readonly ReactiveProperty<ObjectViewModel?> _mine;
        private readonly ObjectUnitCastViewModel _mineUnitCast;

        public ObjectGroupViewModel(IObjectGroupFront objectsFront, IPlayerObjectFront playerObjectFront)
        {
            Debug.Assert(objectsFront.All.Count == 0);

            _all = new ObservableList<ObjectViewModel>();
            _mine = new ReactiveProperty<ObjectViewModel?>().AddTo(_disposables);
            _mineUnitCast = new ObjectUnitCastViewModel(playerObjectFront.Cast, playerObjectFront.Trait).AddTo(_disposables);

            objectsFront.All.ObserveAdd().Subscribe(x =>
            {
                var viewModel = new ObjectViewModel(x.Value);

                viewModel.Unit.Relation
                    .Subscribe(x =>
                    {
                        if (x == ObjectRelation.Mine)
                        {
                            _mine.Value = viewModel;
                        }
                        else
                        {
                            if (_mine.Value == viewModel)
                            {
                                _mine.Value = null;
                            }
                        }
                    });
                
                _all.Insert(x.Index, viewModel);
            }).AddTo(_disposables);

            objectsFront.All.ObserveMove().Subscribe(x =>
            {
                _all.Move(x.OldIndex, x.NewIndex);
            }).AddTo(_disposables);

            objectsFront.All.ObserveRemove().Subscribe(x =>
            {
                var item = _all[x.Index];
                item.Dispose();

                _all.RemoveAt(x.Index);

                if (_mine.Value == item)
                {
                    _mine.Value = null;
                }
            }).AddTo(_disposables);

            objectsFront.All.ObserveReplace().Subscribe(x =>
            {
                _all[x.Index].Dispose();
                _all[x.Index] = new(x.NewValue);
            }).AddTo(_disposables);

            objectsFront.All.ObserveReset().Subscribe(_ =>
            {
                foreach (var item in _all)
                {
                    item.Dispose();
                }
                _all.Clear();
                _mine.Value = null;
            }).AddTo(_disposables);

            ObjectViewModel? mine = null;
            _mine.Subscribe(x =>
            {
                mine?.Set(null);
                mine = x;
                x?.Set(_mineUnitCast);
            });
        }

        public IObservableCollection<ObjectViewModel> All => _all;

        public ReadOnlyReactiveProperty<ObjectViewModel?> Mine => _mine;

        public void Dispose()
        {
            foreach (var item in _all)
            {
                item.Dispose();
            }
            _disposables.Dispose();
        }
    }
}
