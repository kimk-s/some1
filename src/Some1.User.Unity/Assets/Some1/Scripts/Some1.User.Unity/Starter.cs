#if UNITY_EDITOR
#define MODE_DEBUG
#endif

using System;
using Cysharp.Threading.Tasks;
using R3;
using Some1.User.Unity.Utilities;
using Unity.Linq;
using UnityEngine;

namespace Some1.User.Unity
{
    public class Starter : MonoBehaviour, IBackable
    {
        public Program program;
        public GameObject page;
        public GameObject popups;
        public StarterItem developmentItem;
        public StarterItem stagingItem;
        public StarterItem productionItem;

        public bool Back() => BackUtility.Back(popups);

#if MODE_DEBUG
        private AsyncLazy<StarterError> _errorPrefab;

        private void Start()
        {
            page.SetActive(true);

            _errorPrefab = new(() =>
                ResourcesUtility.LoadAsync<StarterError>($"Prefabs/{nameof(StarterError)}", destroyCancellationToken));

            foreach (var item in new[] { developmentItem, stagingItem, productionItem })
            {
                item.startButton.OnClickAsObservable().SubscribeAwait(
                    async (_, ct) =>
                    {
                        try
                        {
                            var args = item.GetArgs();
                            if (!args.Configuration.InMemory)
                            {
                                new Uri(args.Configuration.WaitServerAddress);
                            }
                            item.Save();
                            StartProgram(args);
                        }
                        catch (Exception ex)
                        {
                            popups.Add(await _errorPrefab).Setup(ex);
                        }
                    },
                    AwaitOperation.Drop);
            }
        }
#else
        private void Start()
        {
            StartProgram(new(
                ProgramEnvironment.Production,
                new(
                    false,
                    "http://13.125.216.134:5000")
                ));
        }
#endif

        private void StartProgram(ProgramArgs args)
        {
            program.Setup(args);
            program.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
