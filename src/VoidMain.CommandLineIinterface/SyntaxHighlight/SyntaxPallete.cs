using System;
using System.Collections;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class SyntaxPallete<TColor> : IEnumerable<KeyValuePair<SyntaxClass, Tuple<TColor, TColor>>>
    {
        private readonly Dictionary<SyntaxClass, Tuple<TColor, TColor>> _colors;

        public TColor DefaultBackground { get; }
        public TColor DefaultForeground { get; }

        public SyntaxPallete(
            TColor defaultBackground = default(TColor),
            TColor defaultForeground = default(TColor))
        {
            _colors = new Dictionary<SyntaxClass, Tuple<TColor, TColor>>();
            DefaultBackground = defaultBackground;
            DefaultForeground = defaultForeground;
        }

        public void Add(SyntaxClass @class, TColor background, TColor foreground)
        {
            _colors.Add(@class, Tuple.Create(background, foreground));
        }

        public void GetColors(SyntaxClass @class, out TColor background, out TColor foreground)
        {
            if (_colors.TryGetValue(@class, out Tuple<TColor, TColor> colors))
            {
                background = colors.Item1;
                foreground = colors.Item2;
            }
            else
            {
                background = DefaultBackground;
                foreground = DefaultForeground;
            }
        }

        public IEnumerator<KeyValuePair<SyntaxClass, Tuple<TColor, TColor>>> GetEnumerator()
        {
            return _colors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
