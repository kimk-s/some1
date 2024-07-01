using System;
using System.Collections.Generic;

namespace R3
{
    public static class ReactiveR3Extensions
    {
        public static T AddTo<T>(this T disposable, ref DisposableBuilder builder) where T : IDisposable
        {
            builder.Add(disposable);
            return disposable;
        }

        public static T AddTo<T>(this T disposable, ref DisposableBag bag) where T : IDisposable
        {
            bag.Add(disposable);
            return disposable;
        }

        public static T AddTo<T>(this T disposable, ICollection<IDisposable> disposables) where T : IDisposable
        {
            disposables.Add(disposable);
            return disposable;
        }

        /// <summary>
        /// Lastest values of each sequence are all true.
        /// </summary>
        public static Observable<bool> CombineLatestValuesAreAllTrue(this IEnumerable<Observable<bool>> sources)
        {
            return Observable.CombineLatest(sources).Select(xs =>
            {
                foreach (var item in xs)
                {
                    if (item == false)
                        return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Lastest values of each sequence are all false.
        /// </summary>
        public static Observable<bool> CombineLatestValuesAreAllFalse(this IEnumerable<Observable<bool>> sources)
        {
            return Observable.CombineLatest(sources).Select(xs =>
            {
                foreach (var item in xs)
                {
                    if (item == true)
                        return false;
                }
                return true;
            });
        }
    }
}
