using System;
using System.Threading;
using System.Threading.Tasks;
using Some1.UI;

namespace R3
{
    public static partial class UIR3Extensions
    {
        public static ReadOnlyReactiveProperty<T> Connect<T>(this ReadOnlyReactiveProperty<T> source)
            => source.AsObservable()!.ToReadOnlyReactiveProperty()!;

        public static IDisposable SubscribeAwait<T>(this Observable<T> source, ReactiveProperty<bool> sharedCanExecute, Func<T, CancellationToken, ValueTask> onNextAsync, AwaitOperation awaitOperation)
            => source.SubscribeAwaitInternal(sharedCanExecute, onNextAsync, awaitOperation, null);

        public static IDisposable SubscribeAwait<T>(this Observable<T> source, Func<T, CancellationToken, ValueTask> onNextAsync, AwaitOperation awaitOperation, ReactiveProperty<TaskState> taskState)
            => source.SubscribeAwaitInternal(null, onNextAsync, awaitOperation, taskState);

        public static IDisposable SubscribeAwait<T>(this Observable<T> source, ReactiveProperty<bool> sharedCanExecute, Func<T, CancellationToken, ValueTask> onNextAsync, AwaitOperation awaitOperation, ReactiveProperty<TaskState> taskState)
            => source.SubscribeAwaitInternal(sharedCanExecute, onNextAsync, awaitOperation, taskState);

        private static IDisposable SubscribeAwaitInternal<T>(
            this Observable<T> source,
            ReactiveProperty<bool>? sharedCanExecute,
            Func<T, CancellationToken, ValueTask> onNextAsync,
            AwaitOperation awaitOperation,
            ReactiveProperty<TaskState>? taskState)
        {
            return source.SubscribeAwait(
                async (x, ct) =>
                {
                    if (sharedCanExecute?.Value == false)
                    {
                        return;
                    }

                    if (taskState?.Value.IsRunning == true)
                    {
                        throw new InvalidOperationException();
                    }

                    if (sharedCanExecute is not null)
                    {
                        sharedCanExecute.Value = false;
                    }

                    if (taskState is not null)
                    {
                        taskState.Value = new(TaskStateType.Running);
                    }

                    try
                    {
                        await onNextAsync(x, ct);
                    }
                    catch (Exception ex)
                    {
                        if (taskState is not null)
                        {
                            taskState.Value = new(TaskStateType.Faulted, ex);
                        }
                        return;
                    }
                    finally
                    {
                        if (taskState?.Value.IsRunning == true)
                        {
                            taskState.Value = new(TaskStateType.RanToCompletion);
                        }

                        if (sharedCanExecute is not null)
                        {
                            sharedCanExecute.Value = true;
                        }
                    }
                },
                awaitOperation);
        }
    }
}
