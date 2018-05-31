using System.Text;

namespace VoidMain.CommandLineIinterface.IO
{
    public class MessageTemplate
    {
        public Token[] Tokens { get; }

        public MessageTemplate(Token[] tokens)
        {
            Tokens = tokens;
        }

        public abstract class Token { }

        public sealed class TextToken : Token
        {
            public string Text { get; }

            public TextToken(string text)
            {
                Text = text;
            }

            public override string ToString() => Text;
        }

        public sealed class ArgumentToken : Token
        {
            public int Index { get; }
            public int Alignment { get; }
            public string Format { get; }

            public ArgumentToken(int index, int alignment, string format)
            {
                Index = index;
                Alignment = alignment;
                Format = format;
            }

            public override string ToString()
            {
                var result = new StringBuilder();

                result.Append('{')
                    .Append(Index.ToString())
                    .Append(',')
                    .Append(Alignment.ToString())
                    .Append(':')
                    .Append(Format?.ToString())
                    .Append('}');

                return result.ToString();
            }
        }
    }
}
