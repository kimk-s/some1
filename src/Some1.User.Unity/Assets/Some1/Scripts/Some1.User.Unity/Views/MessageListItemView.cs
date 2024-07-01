using Cysharp.Threading.Tasks;
using Some1.Play.Front;
using Some1.Resources;
using Some1.User.ViewModel;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace Some1.User.Unity.Views
{
    public class MessageListItemView : MonoBehaviour, IBackable
    {
        public TMP_Text messageText;
        public Button okButon;

        private MessageListItemViewModel _viewModel;

        public bool Back()
        {
            Destroy(gameObject);
            return true;
        }

        public void Setup(MessageListItemViewModel viewModel)
        {
            _viewModel = viewModel.AddTo(this);
        }

        private void Start()
        {
            switch (_viewModel.Message)
            {
                case PlayFrontError x:
                    {
                        messageText.text = R.GetString(x.GetMessage());
                    }
                    break;
                case string x:
                    {
                        messageText.text = R.GetString(x);
                    }
                    break;
                default:
                    {
                        messageText.text = R.GetString(_viewModel.Message.ToString());
                    }
                    break;
            }

            okButon.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);
        }
    }
}
