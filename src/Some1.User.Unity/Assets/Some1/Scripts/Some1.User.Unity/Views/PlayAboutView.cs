using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayAboutView : MonoBehaviour, IBackable
    {
        public Button upButton;

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        private void Start()
        {
            upButton.OnClickAsObservable().Subscribe(_ => Back());
        }
    }
}
