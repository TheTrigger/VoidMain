using System;
using System.Collections.Generic;
using System.Linq;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineIinterface.Parser;

namespace VoidMain.Application.Commands.Arguments
{
    public class CommandsSemanticModel : ISemanticModel
    {
        private readonly ICommandsCollection _commands;
        private readonly ICollectionConstructorProvider _colCtorProvider;
        private readonly IEqualityComparer<string> _identifierComparer;
        private readonly CommandNameComparer _nameComparer;
        private readonly List<string> _nameBuffer;

        public CommandsSemanticModel(ApplicationModel appModel,
            ICollectionConstructorProvider colCtorProvider,
            CommandLineSyntaxOptions syntaxOptions = null)
        {
            if (appModel == null)
            {
                throw new ArgumentNullException(nameof(appModel));
            }
            if (appModel.Commands == null)
            {
                throw new ArgumentNullException(nameof(appModel) + "." + nameof(appModel.Commands));
            }
            _commands = appModel.Commands;
            _colCtorProvider = colCtorProvider ?? throw new ArgumentNullException(nameof(colCtorProvider));
            _identifierComparer = syntaxOptions?.IdentifierComparer
                ?? CommandLineSyntaxOptions.DefaultIdentifierComparer;
            _nameComparer = new CommandNameComparer(_identifierComparer);
            _nameBuffer = new List<string>();
        }

        public bool HasSubCommand(IReadOnlyList<string> commandName, string subcommand)
        {
            if (commandName == null)
            {
                throw new ArgumentNullException(nameof(commandName));
            }
            if (subcommand == null)
            {
                throw new ArgumentNullException(nameof(subcommand));
            }

            _nameBuffer.Clear();
            _nameBuffer.AddRange(commandName);
            _nameBuffer.Add(subcommand);

            bool hasSubCommand = _commands.Any(_ => _nameComparer.StartsWith(_.Name, _nameBuffer));
            return hasSubCommand;
        }

        public bool TryGetOptionType(IReadOnlyList<string> commandName, string optionName, out Type valueType)
        {
            if (commandName == null)
            {
                throw new ArgumentNullException(nameof(commandName));
            }
            if (optionName == null)
            {
                throw new ArgumentNullException(nameof(optionName));
            }

            var commands = _commands
                .Where(_ => _nameComparer.StartsWith(_.Name, commandName))
                .ToArray();

            if (commands.Length != 1)
            {
                valueType = null;
                return false;
            }

            var option = commands[0].Arguments
                .FirstOrDefault(_ => _.Kind == ArgumentKind.Option && IsNameOrAliasEquals(_, optionName));

            valueType = option?.Type;
            return option != null;
        }

        private bool IsNameOrAliasEquals(ArgumentModel option, string optionName)
        {
            return _identifierComparer.Equals(option.Name, optionName)
                || _identifierComparer.Equals(option.Alias, optionName);
        }

        public bool TryGetOperandType(IReadOnlyList<string> commandName, int operandIndex, out Type valueType)
        {
            if (commandName == null)
            {
                throw new ArgumentNullException(nameof(commandName));
            }
            if (operandIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(operandIndex));
            }

            var commands = _commands
                .Where(_ => _nameComparer.StartsWith(_.Name, commandName))
                .ToArray();

            if (commands.Length != 1)
            {
                valueType = null;
                return false;
            }

            var operands = commands[0].Arguments
                .Where(_ => _.Kind == ArgumentKind.Operand);

            int currentIndex = 0;

            foreach (var operand in operands)
            {
                bool isCollection = _colCtorProvider.TryGetCollectionConstructor(operand.Type, out var colCtor);

                if (isCollection)
                {
                    // Array consumes all operands to the end.
                    valueType = colCtor
                        .GetElementType(operand.Type)
                        .UnwrapIfNullable();
                    return true;
                }

                if (currentIndex == operandIndex)
                {
                    valueType = operand.Type.UnwrapIfNullable();
                    return true;
                }

                currentIndex++;
            }

            valueType = null;
            return false;
        }
    }
}
