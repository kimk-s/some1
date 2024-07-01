using System;
using System.Linq;
using Some1.Localizing;

namespace Some1.Resources
{
    internal class StringManager
    {
        private readonly StringSet[] _stringSets;

        public StringManager()
        {
            _stringSets = EnumForUnity.GetValues<Culture>()
                .Where(x => x != Culture.None)
                .Select(x => StringSetFactory.Create(x))
                .ToArray();
        }

        public string Get(string id, Culture culture)
        {
            if (culture == Culture.None)
            {
                throw new ArgumentOutOfRangeException(nameof(culture));
            }

            return _stringSets[(int)culture - 1].Get(id);
        }
    }
}
