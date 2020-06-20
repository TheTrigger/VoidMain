using System.Collections.Generic;

namespace VoidMain.Text.Templates.Formatter
{
    public struct DictionaryValueSource : IValueSource<string>
    {
        private readonly Dictionary<string, object> _values;

        public DictionaryValueSource(Dictionary<string, object> values)
        {
            _values = values;
        }

        public object GetValue(string key)
        {
            return _values[key];
        }
    }
}
