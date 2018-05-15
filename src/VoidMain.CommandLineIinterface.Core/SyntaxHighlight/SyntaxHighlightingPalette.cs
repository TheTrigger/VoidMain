using System.Collections;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class SyntaxHighlightingPalette<TStyle> : IEnumerable<KeyValuePair<SyntaxClass, TStyle>>
    {
        private readonly Dictionary<SyntaxClass, TStyle> _styles;

        public TStyle DefaultStyle { get; }

        public SyntaxHighlightingPalette(TStyle defaultStyle = default(TStyle))
        {
            _styles = new Dictionary<SyntaxClass, TStyle>();
            DefaultStyle = defaultStyle;
        }

        public void Add(SyntaxClass @class, TStyle style)
        {
            _styles.Add(@class, style);
        }

        public void GetStyle(SyntaxClass @class, out TStyle style)
        {
            if (!_styles.TryGetValue(@class, out style))
            {
                style = DefaultStyle;
            }
        }

        public IEnumerator<KeyValuePair<SyntaxClass, TStyle>> GetEnumerator()
        {
            return _styles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
