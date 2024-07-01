using System;
using System.Collections.Generic;
using R3;
using Some1.Play.Front;
using Some1.Play.Info;

namespace Some1.User.ViewModel
{
    public class ObjectUnitEnergyGroupViewModel : IDisposable
    {
        private readonly IDisposable _disposable;

        public ObjectUnitEnergyGroupViewModel(IObjectFront front)
        {
            CharacterType = front.Character.Info.Select(x => x?.Type).ToReadOnlyReactiveProperty();

            Energies = front.Energies;

            HitsDamageMinimumCycles = front.Hits.DamageMinimumCycles.Connect().ToReadOnlyReactiveProperty();

            _disposable = Disposable.Combine(CharacterType, HitsDamageMinimumCycles);
        }

        public ReadOnlyReactiveProperty<CharacterType?> CharacterType { get; }

        public IReadOnlyDictionary<EnergyId, IObjectEnergyFront> Energies { get; }

        public ReadOnlyReactiveProperty<float?> HitsDamageMinimumCycles { get; }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
