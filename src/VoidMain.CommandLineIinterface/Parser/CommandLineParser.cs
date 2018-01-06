﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VoidMain.CommandLineIinterface.Internal;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.Parser
{
    public class CommandLineParser : ICommandLineParser
    {
        private readonly ICommandLineLexer _lexer;
        private readonly ISemanticModel _semanticModel;
        private readonly SyntaxFactory _syntaxFactory;

        public CommandLineParser(ICommandLineLexer lexer,
            ISemanticModel semanticModel, SyntaxFactory syntaxFactory)
        {
            _lexer = lexer ?? throw new ArgumentNullException(nameof(lexer));
            _semanticModel = semanticModel ?? throw new ArgumentNullException(nameof(semanticModel));
            _syntaxFactory = syntaxFactory ?? throw new ArgumentNullException(nameof(syntaxFactory));
        }

        public CommandLineSyntax Parse(string commandLine,
            CancellationToken cancellation = default(CancellationToken))
        {
            if (commandLine == null)
            {
                throw new ArgumentNullException(nameof(commandLine));
            }

            var tokens = _lexer.Lex(commandLine, cancellation).ToArray();
            var terminalElement = GetTerminalElement(tokens, commandLine);
            var cursor = new ElementsCursor<SyntaxToken>(tokens, terminalElement);
            var errors = new List<SyntaxError>();

            var commandNameSyntax = ScanCommandName(cursor, errors, cancellation, out IReadOnlyList<string> commandName);
            var argumentsSectionSyntax = ScanArgumentsSection(cursor, errors, cancellation, commandName);
            var endOfInputToken = ScanEndOfInput(cursor, errors);

            cancellation.ThrowIfCancellationRequested();
            return new CommandLineSyntax(commandNameSyntax, argumentsSectionSyntax, endOfInputToken, errors);
        }

        private SyntaxToken GetTerminalElement(SyntaxToken[] tokens, string commandLine)
        {
            var terminalElement = tokens.LastOrDefault();

            if (terminalElement == null || terminalElement.Kind != SyntaxKind.EndOfInputToken)
            {
                var builder = new SyntaxTokenBuilder();
                _syntaxFactory.EndOfInputToken(builder,
                    commandLine, terminalElement?.FullSpan.End ?? 0, missing: true);
                terminalElement = builder.Build();
            }

            return terminalElement;
        }

        private CommandNameSyntax ScanCommandName(
            ElementsCursor<SyntaxToken> cursor, List<SyntaxError> errors, CancellationToken cancellation,
            out IReadOnlyList<string> commandName)
        {
            var nameParts = new List<SyntaxToken>();
            var command = new List<string>();

            while (HasMoreTokens(cursor) && !cancellation.IsCancellationRequested)
            {
                var token = cursor.Peek();
                if (!IsIdentifier(token)) break;

                string subcommand = token.StringValue;
                if (!_semanticModel.HasSubCommand(command, subcommand)) break;

                if (!HasSpaceAfterOrEnd(token, () => cursor.Peek(1)))
                {
                    var builder = new SyntaxTokenBuilder(token);
                    builder.TrailingTrivia = _syntaxFactory.WhitespaceTriviaAfter(token, length: 0, missing: true);
                    token = builder.Build();

                    errors.Add(_syntaxFactory.MissingWhitespaceError(token.TrailingTrivia.Span));
                }

                command.Add(subcommand);
                nameParts.Add(token);
                cursor.MoveNext();
            }

            cancellation.ThrowIfCancellationRequested();
            commandName = command;
            if (nameParts.Count == 0) return null;
            return new CommandNameSyntax(nameParts);
        }

        private ArgumentsSectionSyntax ScanArgumentsSection(
            ElementsCursor<SyntaxToken> cursor, List<SyntaxError> errors, CancellationToken cancellation,
            IReadOnlyList<string> commandName)
        {
            var arguments = new List<ArgumentSyntax>();
            bool operandsOnly = false;
            int operandIndex = 0;

            while (HasMoreTokens(cursor) && !cancellation.IsCancellationRequested)
            {
                var token = cursor.Peek();

                if (!operandsOnly && IsOperandsSectionMarker(token, () => cursor.Peek(1)))
                {
                    cursor.MoveNext();
                    var operandsSectionMarker = new OperandsSectionMarkerSyntax(token);
                    arguments.Add(operandsSectionMarker);
                    operandsOnly = true;
                }
                else if (!operandsOnly && IsOptionName(token, () => cursor.Peek(1)))
                {
                    var nameMarker = token;
                    var name = cursor.Peek(1);
                    cursor.MoveNext(2);
                    var option = ScanOption(cursor, errors, nameMarker, name, commandName);
                    arguments.Add(option);
                }
                else
                {
                    var operand = ScanOperand(cursor, errors, operandIndex, commandName);
                    arguments.Add(operand);
                    operandIndex++;
                }
            }

            cancellation.ThrowIfCancellationRequested();
            if (arguments.Count == 0) return null;
            return new ArgumentsSectionSyntax(arguments);
        }

        // TODO: Add support for stacked short options.
        private OptionSyntax ScanOption(ElementsCursor<SyntaxToken> cursor, List<SyntaxError> errors,
            SyntaxToken nameMarker, SyntaxToken name, IReadOnlyList<string> commandName)
        {
            SyntaxToken valueMarker = null;
            ValueSyntax value = null;

            if (!_semanticModel.TryGetOptionType(commandName, name.StringValue, out Type valueType))
            {
                errors.Add(_syntaxFactory.UnknownOptionNameError(name.Span));
            }

            if (!HasMoreTokens(cursor))
            {
                return new OptionSyntax(nameMarker, name, valueMarker, value);
            }

            var token = cursor.Peek();

            if (IsOptionValueMarker(token))
            {
                valueMarker = token;
                cursor.MoveNext();
                token = cursor.Peek();
            }
            else
            {
                if (!HasSpaceAfterOrEnd(name, () => token))
                {
                    var builder = new SyntaxTokenBuilder(name);
                    builder.TrailingTrivia = _syntaxFactory.WhitespaceTriviaAfter(name, length: 0, missing: true);
                    name = builder.Build();

                    errors.Add(_syntaxFactory.MissingWhitespaceError(name.TrailingTrivia.Span));
                }

                if (IsFlag(valueType))
                {
                    // Flag can have a value only if a value marker is specified
                    // because it have value 'true' by default.
                    return new OptionSyntax(nameMarker, name, valueMarker, value);
                }
            }

            bool missingValue = valueMarker != null &&
                (!HasMoreTokens(cursor) || (valueMarker.HasTrailingTrivia && IsOptionNameOrSectionMarker(token, () => cursor.Peek(1))));

            if (missingValue)
            {
                SyntaxToken valueToken = null;
                var builder = new SyntaxTokenBuilder();
                var span = valueMarker.Span;
                _syntaxFactory.LiteralToken(builder, span.Source, span.End, 0, null, missing: true);

                if (valueMarker.HasTrailingTrivia)
                {
                    // Place trailing trivia after missing option value
                    builder.TrailingTrivia = valueMarker.TrailingTrivia;
                    valueToken = builder.Build();

                    // and remove it from value marker.
                    builder.InitializeWith(valueMarker);
                    builder.TrailingTrivia = null;
                    valueMarker = builder.Build();
                }
                else
                {
                    valueToken = builder.Build();
                }

                errors.Add(_syntaxFactory.MissingOptionValueError(valueToken.Span));
                value = new ValueSyntax(valueToken);
                return new OptionSyntax(nameMarker, name, valueMarker, value);
            }

            bool isValue = (valueMarker != null && !valueMarker.HasTrailingTrivia)
                || !IsOptionNameOrSectionMarker(token, () => cursor.Peek(1));

            if (isValue)
            {
                value = ScanValue(cursor, errors);
            }

            return new OptionSyntax(nameMarker, name, valueMarker, value);
        }

        private OperandSyntax ScanOperand(
            ElementsCursor<SyntaxToken> cursor, List<SyntaxError> errors,
            int operandIndex, IReadOnlyList<string> commandName)
        {
            var valueToken = ScanValue(cursor, errors);
            return new OperandSyntax(valueToken, operandIndex);
        }

        private ValueSyntax ScanValue(ElementsCursor<SyntaxToken> cursor, List<SyntaxError> errors)
        {
            var token = cursor.Peek();
            cursor.MoveNext();

            if (token.Kind == SyntaxKind.QuotedLiteralToken || !HasMoreTokens(cursor))
            {
                if (!HasSpaceAfterOrEnd(token, () => cursor.Peek(1)))
                {
                    // Quoted literals do not merge with any other literal
                    // even if they don't have spaces between them.
                    var builder = new SyntaxTokenBuilder(token);
                    builder.TrailingTrivia = _syntaxFactory.WhitespaceTriviaAfter(token, length: 0, missing: true);
                    token = builder.Build();
                    errors.Add(_syntaxFactory.MissingWhitespaceError(token.TrailingTrivia.Span));
                }

                return new ValueSyntax(token);
            }

            return ScanMultiTokenValue(cursor, errors, token);
        }

        private ValueSyntax ScanMultiTokenValue(
            ElementsCursor<SyntaxToken> cursor, List<SyntaxError> errors,
            SyntaxToken first)
        {
            var tokens = new List<SyntaxToken>();
            var token = first;
            tokens.Add(token);

            while (HasMoreTokens(cursor))
            {
                var next = cursor.Peek();
                if (HasSpaceAfterOrEnd(token, () => next)) break;

                if (next.Kind == SyntaxKind.QuotedLiteralToken)
                {
                    // Quoted literals do not merge with any other literal
                    // even if they don't have spaces between them.
                    var builder = new SyntaxTokenBuilder(token);
                    builder.TrailingTrivia = _syntaxFactory.WhitespaceTriviaAfter(token, length: 0, missing: true);
                    tokens[tokens.Count - 1] = builder.Build();

                    errors.Add(_syntaxFactory.MissingWhitespaceError(builder.TrailingTrivia.Span));
                    break;
                }

                token = next;
                tokens.Add(token);
                cursor.MoveNext();
            }

            return new ValueSyntax(tokens);
        }

        private SyntaxToken ScanEndOfInput(
            ElementsCursor<SyntaxToken> cursor, List<SyntaxError> errors)
        {
            var token = cursor.Peek();
            if (token.Kind == SyntaxKind.EndOfInputToken)
            {
                return token;
            }

            var builder = new SyntaxTokenBuilder();
            var span = token.FullSpan;
            _syntaxFactory.EndOfInputToken(builder, span.Source, span.Start, missing: true);

            errors.Add(_syntaxFactory.MissingEndOfInputError(builder.Span));

            if (!span.IsEmpty)
            {
                int start = span.Start;
                int length = cursor.TerminalElement.FullSpan.End - start;
                builder.TrailingTrivia = _syntaxFactory.UndefinedTrivia(span.Source, start, length);

                errors.Add(_syntaxFactory.UnrecognizedInputError(builder.TrailingTrivia.Span));
            }

            return builder.Build();
        }

        private bool IsIdentifier(SyntaxToken token)
        {
            return token.Kind.HasFlag(SyntaxKind.IdentifierToken);
        }

        private bool IsOperandsSectionMarker(SyntaxToken token, Func<SyntaxToken> getNextToken)
        {
            return token.Kind == SyntaxKind.DashDashToken
                && HasSpaceAfterOrEnd(token, getNextToken);
        }

        private bool IsOptionNameMarker(SyntaxToken token)
        {
            return (token.Kind == SyntaxKind.DashToken || token.Kind == SyntaxKind.DashDashToken);
        }

        private bool IsOptionValueMarker(SyntaxToken token)
        {
            return token.Kind == SyntaxKind.EqualsToken || token.Kind == SyntaxKind.ColonToken;
        }

        private bool IsOptionName(SyntaxToken token, Func<SyntaxToken> getNextToken)
        {
            var nameMarker = token;
            if (!IsOptionNameMarker(nameMarker)) return false;

            var name = getNextToken();
            if (HasSpaceAfterOrEnd(nameMarker, () => name)) return false;

            return IsIdentifier(name);
        }

        private bool IsOptionNameOrSectionMarker(SyntaxToken token, Func<SyntaxToken> getNextToken)
        {
            return IsOptionName(token, getNextToken) || IsOperandsSectionMarker(token, getNextToken);
        }

        private bool IsFlag(Type valueType)
        {
            return valueType == typeof(bool);
        }

        private bool HasSpaceAfterOrEnd(SyntaxToken token, Func<SyntaxToken> getNextToken)
        {
            if (token.HasTrailingTrivia && !token.TrailingTrivia.Span.IsEmpty) return true;
            var nextToken = getNextToken();
            if (nextToken.Kind == SyntaxKind.EndOfInputToken) return true;
            return nextToken.HasLeadingTrivia && !nextToken.LeadingTrivia.Span.IsEmpty;
        }

        private bool HasMoreTokens(ElementsCursor<SyntaxToken> cursor)
        {
            return !cursor.IsAtTheEnd() && cursor.Peek().Kind != cursor.TerminalElement.Kind;
        }
    }
}
