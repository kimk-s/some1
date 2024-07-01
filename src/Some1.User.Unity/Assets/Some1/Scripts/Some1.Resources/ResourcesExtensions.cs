using System;
using System.Drawing;
using R3;

namespace Some1.Resources
{
    public static class ResourcesExtensions
    {
        public static Observable<string> AsRStringObservable(this string id)
        {
            return R.Culture.Select(culture => R.GetString(id, culture));
        }

        public static Observable<string> AsRStringObservable(this Observable<string> id)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return id.CombineLatest(R.Culture, (name, culture) => R.GetString(name, culture));
        }

        public static Observable<string> AsRFilePathObservable(this string id)
        {
            return R.Culture.Select(culture => R.GetFilePath(id, culture));
        }

        public static Observable<string> AsRFilePathObservable(this Observable<string>
            id)
        {
            return id.CombineLatest(R.Culture, (id, culture) => R.GetFilePath(id, culture));
        }

        public static Observable<Color> AsRColorObservable(this ColorId id)
        {
            return R.Theme.Select(theme => R.GetColor(id, theme));
        }

        public static Observable<Color> AsRColorObservable(this Observable<ColorId> id)
        {
            return id.CombineLatest(R.Theme, (id, theme) => R.GetColor(id, theme));
        }
    }
}
