using Cysharp.Threading.Tasks;
using R3;
using Some1.Play.Info;
using Some1.Resources;
using Some1.User.Unity.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerGameResultCharacterView : MonoBehaviour
    {
        public Image iconImage;
        public TMP_Text primaryText;
        public TMP_Text secondaryText;

        private ReadOnlyReactiveProperty<GameResultCharacter?> _viewModel;

        public void Setup(ReadOnlyReactiveProperty<GameResultCharacter?> viewModel)
        {
            _viewModel = viewModel.AddTo(this);
        }

        private void Start()
        {
            _viewModel
                .Select(x => x?.Id)
                .Subscribe(x => LoadIconImageAsync(x).Forget())
                .AddTo(this);

            _viewModel
                .Select(x => x?.Id.GetName())
                .AsRStringObservable()
                .SubscribeToText(primaryText)
                .AddTo(this);

            R.Culture
                .CombineLatest(
                    _viewModel.Select(x => x?.Exp ?? 0),
                    (_, exp) => StringFormatter.FormatScore(exp))
                .SubscribeToText(secondaryText)
                .AddTo(this);
        }

        private async UniTaskVoid LoadIconImageAsync(CharacterId? id)
        {
            iconImage.sprite = null;

            if (id is null)
            {
                return;
            }

            var sprite = await ResourcesUtility.LoadSafeAsync<Sprite>(id.Value.GetIconPath(), destroyCancellationToken);

            if (id != _viewModel.CurrentValue.Value.Id)
            {
                return;
            }

            iconImage.sprite = sprite;
        }
    }
}
