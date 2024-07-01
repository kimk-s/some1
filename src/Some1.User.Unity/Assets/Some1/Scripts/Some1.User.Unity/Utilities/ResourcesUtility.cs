using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Some1.User.Unity.Utilities
{
    public static class ResourcesUtility
    {
        private static readonly Dictionary<string, Task<Object>> _pathToTask = new();

        public static UniTask<T> LoadViewAsync<T>(CancellationToken cancellationToken = default) where T : Object
        {
            return LoadViewAsync<T>(typeof(T).Name, cancellationToken);
        }

        private static UniTask<T> LoadViewAsync<T>(string path, CancellationToken cancellationToken = default) where T : Object
        {
            return LoadAsync<T>($"Prefabs/Views/{path}", cancellationToken);
        }

        public static UniTask<T> LoadAsync<T>(CancellationToken cancellationToken = default) where T : Object
        {
            return LoadAsync<T>(typeof(T).Name, cancellationToken);
        }

        public static async UniTask<T> LoadSafeAsync<T>(string path, CancellationToken cancellationToken = default) where T : Object
        {
            try
            {
                var result = await LoadAsync<T>(path, cancellationToken);
                if (result == null)
                {
                    Debug.LogWarning($"null load resource '{path}'");
                }
                return result;
            }
            catch (System.OperationCanceledException)
            {
            }
            catch (System.Exception ex)
            {
                Debug.LogError(@$"Failed to load resource '{path}'
{ex}");
            }
            return null;
        }

        public static async UniTask<T> LoadAsync<T>(string path, CancellationToken cancellationToken = default) where T : Object
        {
            if (!_pathToTask.TryGetValue(path, out var task))
            {
                task = UnityEngine.Resources.LoadAsync<T>(path).WithCancellation(cancellationToken).AsTask();
                _pathToTask.Add(path, task);
            }

            return (T)await task;
        }
    }
}
