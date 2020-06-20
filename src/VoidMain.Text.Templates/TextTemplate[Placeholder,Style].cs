using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VoidMain.Text.Templates
{
    public class TextTemplate<TPlaceholder, TStyle> : IReadOnlyList<TextTemplate<TPlaceholder, TStyle>.Token>
    {
        private readonly List<Token> _tokens;

        public TextTemplate()
        {
            _tokens = new List<Token>();
        }

        public TextTemplate(int capacity)
        {
            _tokens = new List<Token>(capacity);
        }

        public TextTemplate(IEnumerable<Token> tokens)
        {
            _tokens = tokens.ToList();
        }

        public TextTemplate(List<Token> tokens, bool takeOwnership = false)
        {
            _tokens = takeOwnership ? tokens : tokens.ToList();
        }

        public Token this[int index]
        {
            get => _tokens[index];
            set => _tokens[index] = value;
        }

        public int Count => _tokens.Count;

        public List<Token>.Enumerator GetEnumerator() => _tokens.GetEnumerator();

        IEnumerator<Token> IEnumerable<Token>.GetEnumerator() => _tokens.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _tokens.GetEnumerator();

        public int IndexOf(Token token)
        {
            return _tokens.IndexOf(token);
        }

        public void Add(Token token)
        {
            _tokens.Add(token);
        }

        public void Add(ReadOnlyMemory<char> text, TStyle style = default)
        {
            _tokens.Add(new TextToken(text, style));
        }

        public void Add(string text, TStyle style = default)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            _tokens.Add(new TextToken(text.AsMemory(), style));
        }

        public void Add(TPlaceholder placeholder, TStyle style = default)
        {
            if (placeholder == null)
            {
                throw new ArgumentNullException(nameof(placeholder));
            }
            _tokens.Add(new PlaceholderToken(placeholder, style));
        }

        public void InsertAt(int index, ReadOnlyMemory<char> text, TStyle style = default)
        {
            _tokens.Insert(index, new TextToken(text, style));
        }

        public void InsertAt(int index, string text, TStyle style = default)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            _tokens.Insert(index, new TextToken(text.AsMemory(), style));
        }

        public void InsertAt(int index, TPlaceholder placeholder, TStyle style = default)
        {
            if (placeholder == null)
            {
                throw new ArgumentNullException(nameof(placeholder));
            }
            _tokens.Insert(index, new PlaceholderToken(placeholder, style));
        }

        public void RemoveAt(Token token)
        {
            _tokens.Remove(token);
        }

        public void RemoveAt(int index)
        {
            _tokens.RemoveAt(index);
        }

        public void Clear()
        {
            _tokens.Clear();
        }

        public void Accept<TVisitor>(TVisitor visitor)
            where TVisitor : ITextTemplateVisitor<TPlaceholder, TStyle>
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.BeforeVisitAll();

            foreach (var token in _tokens)
            {
                switch (token)
                {
                    case TextToken tt:
                        visitor.Visit(tt.Text, tt.Style);
                        break;
                    case PlaceholderToken pt:
                        visitor.Visit(pt.Placeholder, pt.Style);
                        break;
                    default:
                        token.Accept(ref visitor);
                        break;
                }
            }

            visitor.AfterVisitAll();
        }

        public abstract class Token
        {
            public TStyle Style { get; set; }

            public abstract void Accept<TVisitor>(ref TVisitor visitor)
                where TVisitor : ITextTemplateVisitor<TPlaceholder, TStyle>;
        }

        public sealed class TextToken : Token
        {
            public ReadOnlyMemory<char> Text { get; set; }

            public TextToken(ReadOnlyMemory<char> text, TStyle style = default)
            {
                Text = text;
                Style = style;
            }

            public sealed override void Accept<TVisitor>(ref TVisitor visitor)
            {
                visitor.Visit(Text, Style);
            }

            public override string ToString() => Text.ToString();
        }

        public sealed class PlaceholderToken : Token
        {
            public TPlaceholder Placeholder { get; set; }

            public PlaceholderToken(TPlaceholder placeholder, TStyle style = default)
            {
                Placeholder = placeholder;
                Style = style;
            }

            public sealed override void Accept<TVisitor>(ref TVisitor visitor)
            {
                visitor.Visit(Placeholder, Style);
            }

            public override string ToString() => Placeholder.ToString();
        }
    }
}
