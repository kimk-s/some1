using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.ViewModel;
using UnityEngine;

namespace Some1.User.Unity.Views
{
    public class ObjectView : MonoBehaviour
    {
        public ObjectUnitView unit;
        public ObjectCharacterView character;
        public ObjectBuffView[] buffs;

        //private ObjectViewModel _viewModel;

        public void Setup(ObjectViewModel viewModel)
        {
            if (viewModel.Buffs.Count != buffs.Length)
            {
                throw new InvalidOperationException();
            }

            unit.Setup(viewModel.Unit);
            character.Setup(viewModel.Character);
            for (int i = 0; i < viewModel.Buffs.Count; i++)
            {
                buffs[i].Setup(viewModel.Buffs[i]);
            }

            character.Size.Subscribe(x => unit.CharacterSize = x).AddTo(this);

            viewModel.Active.SubscribeToActive(gameObject).AddTo(this);

            //_viewModel = viewModel;
        }

        private void Start()
        {
            //_viewModel.Position.Subscribe(x => transform.localPosition = x.ToUnityVector3()).AddTo(this);
        }
    }
}
