using System.Collections.Generic;

namespace Some1
{
    public static class CollectionExtensions
    {
        public static TValue? GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey? key) where TKey : struct
        {
            if (key is null)
            {
                return default;
            }

            return dictionary.GetValueOrDefault(key.Value, default!);
        }
    }
}
