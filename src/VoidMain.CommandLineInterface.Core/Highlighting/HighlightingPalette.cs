using System.Collections;
using System.Collections.Generic;

namespace VoidMain.CommandLineInterface.Highlighting
{
    public class HighlightingPalette<TName, TStyle> : IEnumerable<KeyValuePair<TName, TStyle>>
    {
        private readonly Dictionary<TName, TStyle> _styles;

        public TStyle DefaultStyle { get; set; }

        public HighlightingPalette(TStyle defaultStyle = default(TStyle))
        {
            _styles = new Dictionary<TName, TStyle>();
            DefaultStyle = defaultStyle;
        }

        public void Add(TName name, TStyle style)
        {
            _styles.Add(name, style);
        }

        public void Set(TName name, TStyle style)
        {
            _styles[name] = style;
        }

        public TStyle Get(TName name)
        {
            if (!_styles.TryGetValue(name, out TStyle style))
            {
                style = DefaultStyle;
            }
            return style;
        }

        public void Remove(TName name)
        {
            _styles.Remove(name);
        }

        public IEnumerator<KeyValuePair<TName, TStyle>> GetEnumerator()
        {
            return _styles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
