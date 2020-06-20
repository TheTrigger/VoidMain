namespace VoidMain.Text.Templates.Formatter
{
    public struct ArrayValueSource : IValueSource<int>
    {
        private readonly object[] _values;

        public ArrayValueSource(params object[] values)
        {
            _values = values;
        }

        public object GetValue(int key)
        {
            return _values[key];
        }
    }
}
