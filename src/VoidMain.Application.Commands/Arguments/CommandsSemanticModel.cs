using System;
using System.Collections.Generic;
using System.Linq;
using VoidMain.Application.Commands.Internal;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands.Arguments
{
    public class CommandsSemanticModel : ISemanticModel
    {
        private readonly ApplicationModel _appModel;
        private readonly ICollectionConstructorProvider _colCtorProvider;
        private readonly IServiceProvider _services;
        private readonly CommandLineOptions _cliOptions;
        private readonly CommandNameComparer _nameComparer;
        private readonly List<string> _nameBuffer;

        public CommandsSemanticModel(
            ApplicationModel appModel,
            ICollectionConstructorProvider colCtorProvider,
            IServiceProvider services,
            CommandLineOptions cliOptions = null)
        {
            _appModel = appModel ?? throw new ArgumentNullException(nameof(appModel));
            if (appModel.Commands == null)
            {
                throw new ArgumentNullException(nameof(appModel) + "." + nameof(appModel.Commands));
            }
            _colCtorProvider = colCtorProvider ?? throw new ArgumentNullException(nameof(colCtorProvider));
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _cliOptions = cliOptions ?? new CommandLineOptions();
            _cliOptions.Validate();
            _nameComparer = new CommandNameComparer(_cliOptions.IdentifierComparer);
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

            bool hasSubCommand = _appModel.Commands.Any(_ => _nameComparer.StartsWith(_.Name, _nameBuffer));
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

            var commands = _appModel.Commands
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
            return _cliOptions.IdentifierComparer.Equals(option.Name, optionName)
                || _cliOptions.IdentifierComparer.Equals(option.Alias, optionName);
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

            var commands = _appModel.Commands
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
                bool isCollection = _colCtorProvider.TryGetConstructor(operand.Type, _services, out var colCtor);

                if (isCollection)
                {
                    // Array consumes all operands till the end.
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
