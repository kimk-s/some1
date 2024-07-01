using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.Prefs.UI;
using Some1.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class ThemeView : MonoBehaviour
    {
        public TMP_Text nameText;
        public Button toggleButton;

        private ThemeViewModel _viewModel;

        public void Setup(IServiceProvider services)
        {
            _viewModel = services.GetRequiredService<ThemeViewModel>();
        }

        private void Start()
        {
            _viewModel.Value.Select(x => x.GetName()).AsRStringObservable().SubscribeToText(nameText);

            _viewModel.Toggle.BindTo(toggleButton).AddTo(this);
        }
    }
}
