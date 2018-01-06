﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using VoidMain.CommandLineIinterface.Console;
using VoidMain.CommandLineIinterface.Parser.Syntax;
using VoidMain.Hosting;

namespace VoidMain.CommandLineIinterface
{
    public class ContextCreationHelper
    {
        private readonly StringBuilder _valueBuffer;
        private readonly Dictionary<string, List<string>> _optionsBuffer;
        private readonly List<string> _operandsBuffer;
        private Dictionary<string, object> _context;

        public ContextCreationHelper()
        {
            _valueBuffer = new StringBuilder();
            _optionsBuffer = new Dictionary<string, List<string>>();
            _operandsBuffer = new List<string>();
        }

        public ContextCreationHelper UseContext(Dictionary<string, object> context)
        {
            _context = context;
            return this;
        }

        public ContextCreationHelper SetOutput(ICommandLineOutput output)
        {
            _context[ContextKey.Output] = output;
            return this;
        }

        public ContextCreationHelper SetCancellation(CancellationToken token)
        {
            _context[ContextKey.CommandCanceled] = token;
            return this;
        }

        public ContextCreationHelper SetRawCommandLine(string commandLine)
        {
            _context[ContextKey.CommandLine] = commandLine;
            return this;
        }

        public ContextCreationHelper SetParsedCommandLine(CommandLineSyntax syntax)
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
                _context[ContextKey.CommandOptions] = new Dictionary<string, string[]>();
                _context[ContextKey.CommandOperands] = Array.Empty<string>();
                return;
            }

            var arguments = syntax.Arguments;

            // TODO: Add all lists to pool.
            _optionsBuffer.Clear();
            _operandsBuffer.Clear();

            foreach (var arg in arguments)
            {
                switch (arg)
                {
                    case OptionSyntax option:
                        string optionName = option.Name.StringValue;
                        if (!_optionsBuffer.TryGetValue(optionName, out List<string> values))
                        {
                            // TODO: Use pool for lists.
                            values = new List<string>();
                            _optionsBuffer.Add(optionName, values);
                        }
                        string optionValue = GetValue(option.Value);
                        values.Add(optionValue);
                        break;
                    case OperandSyntax operand:
                        string operandValue = GetValue(operand.Value);
                        _operandsBuffer.Add(operandValue);
                        break;
                    default:
                        break;
                }
            }

            _context[ContextKey.CommandOptions] = _optionsBuffer
                .ToDictionary(kv => kv.Key, kv => kv.Value.ToArray());
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
