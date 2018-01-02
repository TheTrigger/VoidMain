using System;
using System.Collections.Generic;
using System.Text;
using VoidMain.CommandLineIinterface.Internal;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.Parser
{
    public class CommandLineLexer : ICommandLineLexer
    {
        private const char EndOfInput = '\0';

        private readonly SyntaxFactory _syntaxFactory;

        public CommandLineLexer(SyntaxFactory syntaxFactory)
        {
            _syntaxFactory = syntaxFactory ?? throw new ArgumentNullException(nameof(syntaxFactory));
        }

        public IEnumerable<SyntaxToken> Lex(string commandLine)
        {
            if (commandLine == null)
            {
                throw new ArgumentNullException(nameof(commandLine));
            }

            var chars = new CharsReadOnlyList(commandLine);
            var cursor = new ElementsCursor<char>(chars, EndOfInput);

            var builder = new SyntaxTokenBuilder();
            do
            {
                builder.LeadingTrivia = LexSyntaxTrivia(commandLine, cursor);
                LexSyntaxToken(builder, commandLine, cursor);
                builder.TrailingTrivia = LexSyntaxTrivia(commandLine, cursor);
                yield return builder.Build();
            } while (builder.Kind != SyntaxKind.EndOfInputToken);
        }

        private SyntaxTrivia LexSyntaxTrivia(string input, ElementsCursor<char> cursor)
        {
            return ScanWhitespaceTrivia(input, cursor);
        }

        private SyntaxTrivia ScanWhitespaceTrivia(string input, ElementsCursor<char> cursor)
        {
            int start = cursor.Position;
            while (!cursor.IsAtTheEnd())
            {
                var c = cursor.Peek();
                if (!Char.IsWhiteSpace(c)) break;
                cursor.MoveNext();
            }
            if (cursor.Position == start)
            {
                return null;
            }
            return _syntaxFactory.WhitespaceTrivia(input, start, cursor.Position - start);
        }

        private void LexSyntaxToken(
            SyntaxTokenBuilder builder, string input, ElementsCursor<char> cursor)
        {
            switch (cursor.Peek())
            {
                case EndOfInput:
                    _syntaxFactory.EndOfInputToken(builder, input, cursor.Position);
                    break;
                case '-':
                    char next = cursor.Peek(1);
                    if (next == '-')
                    {
                        _syntaxFactory.DashDashToken(builder, input, cursor.Position);
                        cursor.MoveNext(2);
                    }
                    else if (Char.IsDigit(next))
                    {
                        // Possibly a negative number.
                        ScanLiteralOrIdentifierToken(builder, input, cursor);
                    }
                    else
                    {
                        _syntaxFactory.DashToken(builder, input, cursor.Position);
                        cursor.MoveNext();
                    }
                    break;
                case '=':
                    _syntaxFactory.EqualsToken(builder, input, cursor.Position);
                    cursor.MoveNext();
                    break;
                case ':':
                    _syntaxFactory.ColonToken(builder, input, cursor.Position);
                    cursor.MoveNext();
                    break;
                default:
                    ScanLiteralOrIdentifierToken(builder, input, cursor);
                    break;
            }
        }

        private void ScanLiteralOrIdentifierToken(
            SyntaxTokenBuilder builder, string input, ElementsCursor<char> cursor)
        {
            int start = cursor.Position;
            char? quote = null;
            var stringValue = new StringBuilder(128);

            char first = cursor.Peek();
            bool isIdentifier = Char.IsLetter(first);

            if (first == '"' || first == '\'')
            {
                quote = first;
                isIdentifier = false;
                cursor.MoveNext();
            }

            while (!cursor.IsAtTheEnd())
            {
                char c = cursor.Peek();

                if (Char.IsWhiteSpace(c))
                {
                    if (!quote.HasValue) break;
                    stringValue.Append(c);
                    cursor.MoveNext();
                }
                else if (c == '=' || c == ':')
                {
                    if (isIdentifier) break;
                    stringValue.Append(c);
                    cursor.MoveNext();
                }
                else if (c == '"' || c == '\'')
                {
                    // Start of another quoted literal.
                    if (!quote.HasValue) break;

                    if (c == quote)
                    {
                        char next = cursor.Peek(1);
                        if (next == c)
                        {
                            // Escaped quote sequence.
                            stringValue.Append(c);
                            cursor.MoveNext(2);
                        }
                        else
                        {
                            // Closing quote symbol.
                            cursor.MoveNext();
                            break;
                        }
                    }
                    else
                    {
                        // Another quote symbol.
                        stringValue.Append(c);
                        cursor.MoveNext();
                    }
                }
                else
                {
                    if (isIdentifier && !IsIdentifierSymbol(c))
                    {
                        isIdentifier = false;
                    }
                    stringValue.Append(c);
                    cursor.MoveNext();
                }
            }

            // We don't know for sure that this is a pure identifier without a context
            // but we definitely can identify a pure literal.
            if (isIdentifier)
            {
                _syntaxFactory.IdentifierOrLiteralToken(builder, input,
                    start, cursor.Position - start, stringValue.ToString());
            }
            else
            {
                _syntaxFactory.LiteralToken(builder, input,
                    start, cursor.Position - start, stringValue.ToString(), quoted: quote.HasValue);
            }
        }

        private bool IsIdentifierSymbol(char symbol)
        {
            // First symbol must be a letter but it tested separately.
            return Char.IsLetterOrDigit(symbol)
                || symbol == '_'
                || symbol == '-'
                || symbol == '.';
        }
    }
}
