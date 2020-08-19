using System;

namespace VoidMain.Text.Templates.Parser
{
    public class TextTemplateParser<TPlaceholder> : ITextTemplateParser<TPlaceholder>
    {
        private readonly IPlaceholderParser<TPlaceholder> _placeholderParser;
        private readonly ParseRange _placeholderRange;

        public TextTemplateParser(IPlaceholderParser<TPlaceholder> placeholderParser)
        {
            _placeholderParser = placeholderParser ?? throw new ArgumentNullException(nameof(placeholderParser));
            _placeholderRange = new ParseRange('}');
        }

        public void Parse<TVisitor>(string templateText, ref TVisitor visitor)
            where TVisitor : ITextTemplateVisitor<TPlaceholder>
        {
            if (templateText == null)
            {
                throw new ArgumentNullException(nameof(templateText));
            }

            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            if (templateText.Length == 0)
            {
                return;
            }

            int position = 0;
            visitor.BeforeVisitAll();

            while (position < templateText.Length)
            {
                switch (templateText[position])
                {
                    case '{':
                        position++;

                        if (position >= templateText.Length)
                        {
                            throw new FormatException(); // Unexpected end of template string
                        }

                        if (templateText[position] == '{')
                        {
                            visitor.Visit(templateText.AsMemory(position, 1));
                            position++;
                        }
                        else
                        {
                            int consumed = _placeholderParser.Parse(
                                templateText, position, _placeholderRange, out var placeholder);

                            visitor.Visit(placeholder);
                            position += consumed;

                            if (position >= templateText.Length)
                            {
                                throw new FormatException(); // Expected placeholder closing bracket but received end of input
                            }

                            if (templateText[position] != '}')
                            {
                                throw new FormatException(); // Expected placeholder closing bracket
                            }

                            position++;
                        }
                        break;

                    case '}':
                        position++;

                        if (position >= templateText.Length)
                        {
                            throw new FormatException(); // Unexpected end of template string
                        }

                        if (templateText[position] != '}')
                        {
                            throw new FormatException(); // Expected escaped closing bracket but received end of input
                        }

                        visitor.Visit(templateText.AsMemory(position, 1));
                        position++;
                        break;

                    default:
                        int start = position;
                        do { position++; }
                        while (position < templateText.Length && !IsControl(templateText[position]));
                        visitor.Visit(templateText.AsMemory(start, position - start));
                        break;
                }
            }

            visitor.AfterVisitAll();
        }

        private static bool IsControl(char symbol)
            => symbol == '{' || symbol == '}' || symbol == '[' || symbol == ']';
    }
}
