using System;
using Cysharp.Threading.Tasks;
using Some1.User.Unity;
using Some1.User.Unity.Components;
using TMPro;
using Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace R3
{
    internal static partial class UnityR3Extensions
    {
        public static IDisposable SubscribeToDestroy(this Observable<bool> source, GameObject gameObject)
        {
            return source.Where(x => x).Take(1).Subscribe(gameObject, static (_, s) => s.Destroy());
        }

        public static IDisposable SubscribeToActive(this Observable<bool> source, GameObject gameObject)
        {
            return source.Subscribe(gameObject, static (x, s) => s.SetActive(x));
        }

        public static IDisposable SubscribeToText(this Observable<string?> source, TMP_Text text)
        {
            return source.Subscribe(text, static (x, t) => t.text = x);
        }

        public static IDisposable SubscribeToText<T>(this Observable<T?> source, TMP_Text text)
        {
            return source.Subscribe(text, static (x, t) => t.text = x?.ToString());
        }

        public static IDisposable SubscribeToText<T>(this Observable<T> source, TMP_Text text, Func<T, string?> selector)
        {
            return source.Subscribe((text, selector), static (x, state) => state.text.text = state.selector(x));
        }

        public static IDisposable SubscribeToText(this Observable<string?> source, TextCulture text)
        {
            return source.Subscribe(text, static (x, t) => t.StringId = x);
        }

        public static IDisposable SubscribeToText<T>(this Observable<T?> source, TextCulture text)
        {
            return source.Subscribe(text, static (x, t) => t.StringId = x?.ToString());
        }

        public static IDisposable SubscribeToText<T>(this Observable<T> source, TextCulture text, Func<T, string?> selector)
        {
            return source.Subscribe((text, selector), static (x, state) => state.text.StringId = state.selector(x));
        }

        public static Observable<JoystickHandledEventArgs> OnHandleAsObservable(this Joystick joystick)
        {
            return joystick.onHandle.AsObservable();
        }

        public static IDisposable BindToOnClick(this Button button, Func<Unit, UniTask> function)
        {
            var command = new ReactiveCommand<Unit>();
            command.SubscribeAwait(async (x, ct) => await function(x));
            var subscription = command.BindTo(button);
            return Disposable.Combine(command, subscription);
        }

        public static IDisposable BindToOnClick(this Button button, ReactiveProperty<bool> sharedCanExecuteSource, Func<Unit, UniTask> function)
        {
            var command = new ReactiveCommand<Unit>(sharedCanExecuteSource, true);
            command.SubscribeAwait(async (x, ct) => await function(x));
            var subscription = command.BindTo(button);
            return Disposable.Combine(command, subscription);
        }

        public static IDisposable BindTo(this ReactiveCommand<Unit> command, Button button)
        {
            return button.OnClickAsObservable().Subscribe(command, (x, c) => c.Execute(x));
        }

        public static IDisposable BindTo<T>(this ReactiveCommand<T> command, Button button, Func<Unit, T> argumentsFactory)
        {
            return button.OnClickAsObservable().Subscribe(command, (x, c) => c.Execute(argumentsFactory(x)));
        }

        public static IDisposable BindTo(this ReactiveProperty<string?> property, TMP_InputField inputField)
        {
            return new CompositeDisposable(
                property.Subscribe(x => inputField.text = x),
                inputField.OnValueChangedAsObservable().Subscribe(x => property.Value = x));
        }

        public static Observable<string> OnEndEditAsObservable(this TMP_InputField inputField)
        {
            return inputField.onEndEdit.AsObservable();
        }

        public static Observable<string> OnValueChangedAsObservable(this TMP_InputField inputField)
        {
            return Observable.Create<string, TMP_InputField>(inputField, (observer, state) =>
            {
                observer.OnNext(state.text);
                return state.onValueChanged.AsObservable().Subscribe(observer);
            });
        }
    }
}
