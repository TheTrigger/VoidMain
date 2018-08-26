using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VoidMain.Application;
using VoidMain.CommandLineInterface.Parser.Syntax;

namespace VoidMain.CommandLineInterface.Internal
{
    public class ContextBuilder
    {
        private readonly List<KeyValuePair<string, string>> _optionsBuffer;
        private readonly List<string> _operandsBuffer;

        private CancellationToken _token;
        private string _rawCommandLine;
        private CommandLineSyntax _parsedCommandLine;

        public ContextBuilder()
        {
            _optionsBuffer = new List<KeyValuePair<string, string>>();
            _operandsBuffer = new List<string>();
        }

        public ContextBuilder SetCancelToken(CancellationToken token)
        {
            _token = token;
            return this;
        }

        public ContextBuilder SetRawCommandLine(string rawCommandLine)
        {
            _rawCommandLine = rawCommandLine;
            return this;
        }

        public ContextBuilder SetParsedCommandLine(CommandLineSyntax parsedCommandLine)
        {
            _parsedCommandLine = parsedCommandLine;
            return this;
        }

        public Dictionary<string, object> Build()
        {
            var context = new Dictionary<string, object>();

            if (_token != null)
            {
                ContextHelper.SetCancelToken(context, _token);
            }
            if (_rawCommandLine != null)
            {
                ContextHelper.SetCommandLine(context, _rawCommandLine);
            }
            if (_parsedCommandLine != null)
            {
                SetCommandName(context, _parsedCommandLine.CommandName);
                SetCommandArguments(context, _parsedCommandLine.Arguments);
            }

            return context;
        }

        private void SetCommandName(Dictionary<string, object> context, CommandNameSyntax syntax)
        {
            if (syntax == null)
            {
                ContextHelper.SetCommandName(context, Array.Empty<string>());
                return;
            }

            var commandName = syntax.NameParts.Select(_ => _.StringValue).ToArray();
            ContextHelper.SetCommandName(context, commandName);
        }

        private void SetCommandArguments(Dictionary<string, object> context, ArgumentsSectionSyntax syntax)
        {
            if (syntax == null)
            {
                ContextHelper.SetOptions(context, Array.Empty<KeyValuePair<string, string>>());
                ContextHelper.SetOperands(context, Array.Empty<string>());
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

            ContextHelper.SetOptions(context, _optionsBuffer.ToArray());
            ContextHelper.SetOperands(context, _operandsBuffer.ToArray());
        }

        private string GetValue(ValueSyntax valueSyntax)
        {
            if (valueSyntax == null || valueSyntax.Tokens.Count == 0)
            {
                return String.Empty;
            }

            if (valueSyntax.Tokens.Count == 1)
            {
                return valueSyntax.Tokens[0].StringValue;
            }

            return String.Join("", valueSyntax.Tokens.Select(_ => _.StringValue));
        }
    }
}
