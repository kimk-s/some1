using System;
using Cysharp.Threading.Tasks;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Some1.User.Unity
{
    public class StarterError : MonoBehaviour, IBackable
    {
        public TMP_Text messageText;
        public Button okButton;

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        public void Setup(Exception ex)
        {
            string x = ex?.ToString();

            if (x is not null)
            {
                Debug.LogError(x);
            }
            messageText.text = x is null || x.Length <= 200 ? x : $"{x[..200]}...";
        }

        private void Start()
        {
            okButton
                .OnClickAsObservable()
                .Subscribe(_ => Back())
                .AddTo(this);
        }
    }
}
