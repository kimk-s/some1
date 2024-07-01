using System.Collections.Generic;

namespace Some1.Resources
{
    internal class StringSet
    {
        private readonly Dictionary<string, string> _table;

        public StringSet(Dictionary<string, string> table)
        {
            _table = table;
        }

        public string Get(string id)
        {
            _table.TryGetValue(id, out var value);
            return value ?? $"<{id}>";
        }
    }
}
