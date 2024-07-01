using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using R3;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayMenuView : MonoBehaviour, IBackable
    {
        private enum NavDestinationId
        {
            Title,
            Characters,
            Specialties,
            Premium,
            _Length
        }

        public GameObject popups;
        public GameObject[] navSelected;
        public MonoBehaviour[] navDestinations;
        public Button[] navGoToButtons;
        public Button closeButton;
        public Button backButton;
        public Button moreButton;
        public PlayMenuMoreView more;
        public PlayUnaryView unary;
        public PlayBuyResultView buyResult;
        public MessageListView messageList;

        private readonly List<NavDestinationId> _navHistory = new();
        private NavDestinationId _navDestinationId = NavDestinationId._Length;
        private bool _isStarted;

        public void Setup(IServiceProvider serviceProvider)
        {
            ((PlayTitleView)navDestinations[(int)NavDestinationId.Title]).Setup(serviceProvider, popups);
            ((PlayerCharacterGroupView)navDestinations[(int)NavDestinationId.Characters]).Setup(serviceProvider, popups);
            ((PlayerSpecialtyGroupView)navDestinations[(int)NavDestinationId.Specialties]).Setup(serviceProvider, popups);
            ((PlayerPremiumView)navDestinations[(int)NavDestinationId.Premium]).Setup(serviceProvider, popups);
            more.Setup(serviceProvider, popups);

            var viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.Menu;

            unary.Setup(viewModel.Unary);
            buyResult.Setup(viewModel.BuyResult);
            messageList.Setup(viewModel.MessageList);
        }

        public bool Back()
        {
            if (gameObject.activeSelf)
            {
                if (messageList.Back())
                {
                    return true;
                }

                if (buyResult.Back())
                {
                    return true;
                }

                if (unary.Back())
                {
                    return true;
                }

                if (BackUtility.Back(popups))
                {
                    return true;
                }

                if (more.Back())
                {
                    return true;
                }

                if (BackNav())
                {
                    return true;
                }

                Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (_isStarted)
            {
                if (_navHistory.Count == 1 && _navHistory[0] != NavDestinationId.Title)
                {
                    _navHistory.Clear();
                    GoToNav(NavDestinationId.Title, true);
                }
            }
        }

        private void Start()
        {
            Assert.AreEqual((int)NavDestinationId._Length, navGoToButtons.Length);
            Assert.AreEqual((int)NavDestinationId._Length, navSelected.Length);
            Assert.AreEqual((int)NavDestinationId._Length, navDestinations.Length);

            for (int i = 0; i < (int)NavDestinationId._Length; i++)
            {
                NavDestinationId id = (NavDestinationId)i;
                navGoToButtons[i].OnClickAsObservable().Subscribe(_ => GoToNav(id, true));
            }

            closeButton.OnClickAsObservable().Subscribe(_ => Close()).AddTo(this);

            backButton.OnClickAsObservable().Subscribe(_ => Back()).AddTo(this);

            moreButton.OnClickAsObservable().Subscribe(_ => more.gameObject.SetActive(!more.gameObject.activeSelf)).AddTo(this);

            GoToNav(NavDestinationId.Title, true);

            _isStarted = true;
        }

        private void GoToNav(NavDestinationId id, bool addNavHistory)
        {
            if (addNavHistory)
            {
                AddNavHistory(id);
            }

            if (_navDestinationId != id)
            {
                if (_navDestinationId != NavDestinationId._Length)
                {
                    navSelected[(int)_navDestinationId].SetActive(false);
                    navDestinations[(int)_navDestinationId].gameObject.SetActive(false);
                }

                {
                    _navDestinationId = id;
                    navSelected[(int)_navDestinationId].SetActive(true);
                    navDestinations[(int)_navDestinationId].gameObject.SetActive(true);
                }
            }
        }

        private void AddNavHistory(NavDestinationId id)
        {
            _navHistory.Remove(id);
            _navHistory.Add(id);
        }

        private bool BackNav()
        {
            if (_navHistory.Count == 1)
            {
                return false;
            }

            _navHistory.RemoveAt(_navHistory.Count - 1);
            var id = _navHistory[^1];
            GoToNav(id, false);
            return true;
        }
    }
}
