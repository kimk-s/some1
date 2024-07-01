using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using R3;
using Some1.User.Unity.Utilities;
using Some1.User.ViewModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Some1.User.Unity.Views
{
    public class PlayerGameMenuView : MonoBehaviour, IBackable
    {
        private enum NavDestinationId
        {
            Argses,
            Results,
            _Length
        }

        public GameObject popups;
        public GameObject[] navSelected;
        public MonoBehaviour[] navDestinations;
        public Button[] navGoToButtons;
        public Button closeButton;
        public Button backButton;

        private readonly List<NavDestinationId> _navHistory = new();
        private PlayerGameMenuViewModel _viewModel;
        private NavDestinationId _navDestinationId = NavDestinationId._Length;
        private bool _isStarted;

        public void Setup(IServiceProvider serviceProvider)
        {
            _viewModel = serviceProvider.GetRequiredService<PlayViewModel>().Popups.GameMenu;
            ((PlayerGameArgsGroupView)navDestinations[(int)NavDestinationId.Argses]).Setup(serviceProvider, popups);
            ((PlayerGameResultGroupView)navDestinations[(int)NavDestinationId.Results]).Setup(serviceProvider, popups);
        }

        public bool Back()
        {
            if (gameObject.activeSelf)
            {
                if (BackUtility.Back(popups))
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

        private void OnEnable()
        {
            if (_isStarted)
            {
                if (_navHistory.Count == 1 && _navHistory[0] != NavDestinationId.Argses)
                {
                    _navHistory.Clear();
                    GoToNav(NavDestinationId.Argses, true);
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

            _viewModel.Close.Subscribe(_ => Close()).AddTo(this);

            GoToNav(NavDestinationId.Argses, true);

            _isStarted = true;
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        public void OpenLastResultDetail()
        {
            GoToNav(NavDestinationId.Results, true);
            ((PlayerGameResultGroupView)navDestinations[(int)NavDestinationId.Results]).OpenLastDetail();
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
