using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VoidMain.CommandLineIinterface.Parser.Syntax;
using VoidMain.Hosting;

namespace VoidMain.CommandLineIinterface.Internal
{
    public class ContextInitHelper
    {
        private readonly StringBuilder _valueBuffer;
        private readonly List<KeyValuePair<string, string>> _optionsBuffer;
        private readonly List<string> _operandsBuffer;
        private Dictionary<string, object> _context;

        public ContextInitHelper()
        {
            _valueBuffer = new StringBuilder();
            _optionsBuffer = new List<KeyValuePair<string, string>>();
            _operandsBuffer = new List<string>();
        }

        public ContextInitHelper UseContext(Dictionary<string, object> context)
        {
            _context = context;
            return this;
        }

        public ContextInitHelper SetCancellation(CancellationToken token)
        {
            _context[ContextKey.CommandCancelToken] = token;
            return this;
        }

        public ContextInitHelper SetRawCommandLine(string commandLine)
        {
            _context[ContextKey.CommandLine] = commandLine;
            return this;
        }

        public ContextInitHelper SetParsedCommandLine(CommandLineSyntax syntax)
        {
            SetCommandName(syntax.CommandName);
            SetCommandArguments(syntax.Arguments);
            return this;
        }

        private void SetCommandName(CommandNameSyntax syntax)
        {
            if (syntax == null)
            {
                _context[ContextKey.CommandName] = Array.Empty<string>();
                return;
            }

            var nameParts = syntax.NameParts;
            var commandName = new string[nameParts.Count];

            for (int i = 0; i < commandName.Length; i++)
            {
                commandName[i] = nameParts[i].StringValue;
            }

            _context[ContextKey.CommandName] = commandName;
        }

        private void SetCommandArguments(ArgumentsSectionSyntax syntax)
        {
            if (syntax == null)
            {
                _context[ContextKey.CommandOptions] = Array.Empty<KeyValuePair<string, string>>();
                _context[ContextKey.CommandOperands] = Array.Empty<string>();
                return;
            }

            var arguments = syntax.Arguments;

            _optionsBuffer.Clear();
            _operandsBuffer.Clear();

            foreach (var arg in arguments)
            {
                switch (arg)
                {
                    case OptionSyntax option:
                        string optionName = option.Name.StringValue;
                        string optionValue = GetValue(option.Value);
                        _optionsBuffer.Add(new KeyValuePair<string, string>(optionName, optionValue));
                        break;
                    case OperandSyntax operand:
                        string operandValue = GetValue(operand.Value);
                        _operandsBuffer.Add(operandValue);
                        break;
                    default:
                        break;
                }
            }

            _context[ContextKey.CommandOptions] = _optionsBuffer.ToArray();
            _context[ContextKey.CommandOperands] = _operandsBuffer.ToArray();
        }

        private string GetValue(ValueSyntax valueSyntax)
        {
            if (valueSyntax == null || valueSyntax.Tokens.Count == 0)
            {
                return null;
            }

            if (valueSyntax.Tokens.Count == 1)
            {
                return valueSyntax.Tokens[0].StringValue;
            }

            _valueBuffer.Clear();

            foreach (var token in valueSyntax.Tokens)
            {
                _valueBuffer.Append(token.StringValue);
            }

            return _valueBuffer.ToString();
        }
    }
}
