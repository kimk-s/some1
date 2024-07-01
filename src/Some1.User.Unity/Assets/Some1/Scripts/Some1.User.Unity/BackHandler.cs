using Some1.User.Unity.Utilities;
using UnityEngine;

namespace Some1.User.Unity
{
    public class BackHandler : MonoBehaviour, IBackable
    {
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Back();
            }
        }

        public bool Back() => BackUtility.Back(GlobalBinding.Instance.CanvasLayer1, GlobalBinding.Instance.CanvasLayer2);
    }
}
