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

            SyntaxToken token = null;
            do
            {
                var leading = LexSyntaxTrivia(commandLine, cursor);
                token = LexSyntaxToken(commandLine, cursor);
                var trailing = LexSyntaxTrivia(commandLine, cursor);

                token.LeadingTrivia = leading;
                token.TrailingTrivia = trailing;

                yield return token;
            } while (token.Kind != SyntaxKind.EndOfInputToken);
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

        private SyntaxToken LexSyntaxToken(string input, ElementsCursor<char> cursor)
        {
            SyntaxToken token = null;

            switch (cursor.Peek())
            {
                case EndOfInput:
                    token = _syntaxFactory.EndOfInputToken(input, cursor.Position);
                    break;
                case '-':
                    char next = cursor.Peek(1);
                    if (next == '-')
                    {
                        token = _syntaxFactory.DashDashToken(input, cursor.Position);
                        cursor.MoveNext(2);
                    }
                    else if (Char.IsDigit(next))
                    {
                        token = ScanPotentialNegativeNumberToken(input, cursor);
                    }
                    else
                    {
                        token = _syntaxFactory.DashToken(input, cursor.Position);
                        cursor.MoveNext();
                    }
                    break;
                case '=':
                    token = _syntaxFactory.EqualsToken(input, cursor.Position);
                    cursor.MoveNext();
                    break;
                case ':':
                    token = _syntaxFactory.ColonToken(input, cursor.Position);
                    cursor.MoveNext();
                    break;
                default:
                    token = ScanLiteralOrIdentifierToken(input, cursor);
                    break;
            }

            return token;
        }

        private SyntaxToken ScanPotentialNegativeNumberToken(string input, ElementsCursor<char> cursor)
        {
            int start = cursor.Position;

            cursor.MoveNext(2);

            while (!cursor.IsAtTheEnd())
            {
                var c = cursor.Peek();
                if(Char.IsWhiteSpace(c)) break;
                cursor.MoveNext();
            }

            int length = cursor.Position - start;

            return _syntaxFactory.IdentifierOrLiteralToken(
                input, start, length, input.Substring(start, length));
        }

        private SyntaxToken ScanLiteralOrIdentifierToken(string input, ElementsCursor<char> cursor)
        {
            int start = cursor.Position;
            bool quoted = false;
            var stringValue = new StringBuilder(128);

            while (!cursor.IsAtTheEnd())
            {
                var c = cursor.Peek();

                if (Char.IsWhiteSpace(c) || c == '=' || c == ':')
                {
                    if (!quoted) break;
                    stringValue.Append(c);
                    cursor.MoveNext();
                }
                else if (c == '"')
                {
                    // escaped quote symbol
                    if (quoted && cursor.Peek(1) == '"')
                    {
                        stringValue.Append('"');
                        cursor.MoveNext(2);
                    }
                    else
                    {
                        quoted = !quoted;
                        cursor.MoveNext();
                    }
                }
                else
                {
                    stringValue.Append(c);
                    cursor.MoveNext();
                }
            }

            return _syntaxFactory.IdentifierOrLiteralToken(
                input, start, cursor.Position - start, stringValue.ToString());
        }
    }
}
