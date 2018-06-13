using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VoidMain.Application.Commands.Arguments.CollectionConstructors;
using VoidMain.Application.Commands.Arguments.ValueParsers;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands.Builder.Validation
{
    public class CommandModelValidator : ICommandModelValidator
    {
        private readonly TypeInfo ValueParserType = typeof(IValueParser).GetTypeInfo();

        private readonly ICollectionConstructorProvider _collectionCtorProvider;
        private readonly CommandLineOptions _cliOptions;
        private readonly IIdentifierValidator _identifierValidator;

        public CommandModelValidator(
            ICollectionConstructorProvider collectionCtorProvider,
            CommandLineOptions cliOptions = null)
        {
            _collectionCtorProvider = collectionCtorProvider ?? throw new ArgumentNullException(nameof(collectionCtorProvider));
            _cliOptions = cliOptions ?? new CommandLineOptions();
            _cliOptions.Validate();
            _identifierValidator = new IdentifierValidator();
        }

        public ValidationResult Validate(CommandModel command)
        {
            var errors = new List<string>();
            ValidateCommand(command, errors);
            ValidateArguments(command.Arguments, errors);
            return new ValidationResult(errors);
        }

        private void ValidateCommand(CommandModel command, List<string> errors)
        {
            var name = command.Name;

            if (name.Parts == null || name.Parts.Count == 0)
            {
                errors.Add("Command name is empty");
            }

            if (name.Parts.Any(_ => !_identifierValidator.IsValid(_)))
            {
                errors.Add("Command name is not a valid identifier");
            }
        }

        private void ValidateArguments(IReadOnlyList<ArgumentModel> arguments, List<string> errors)
        {
            var lastOperand = arguments.LastOrDefault(_ => _.Kind == ArgumentKind.Operand);

            foreach (var arg in arguments)
            {
                if (String.IsNullOrEmpty(arg.Name))
                {
                    errors.Add(GetErrorMessage(arg, "Argument name is empty"));
                }
                if (!_identifierValidator.IsValid(arg.Name))
                {
                    errors.Add(GetErrorMessage(arg, "Argument name is not a valid identifier"));
                }

                if (arg.Alias != null)
                {
                    if (_cliOptions.IdentifierComparer.Equals(arg.Alias, arg.Name))
                    {
                        errors.Add(GetErrorMessage(arg, "Argument alias is the same as the name"));
                    }
                    if (!_identifierValidator.IsValid(arg.Name))
                    {
                        errors.Add(GetErrorMessage(arg, "Argument alias is not a valid identifier"));
                    }
                }

                if (arg.ValueParser != null && !IsValueParser(arg.ValueParser))
                {
                    errors.Add(GetErrorMessage(arg, "Argument value parser must implement `" + nameof(IValueParser) + "` interface"));
                }

                if (lastOperand != null && IsCollectionOperand(arg) && arg != lastOperand)
                {
                    errors.Add(GetErrorMessage(arg, "Operand with a collection of values must be the last one"));
                }
            }
        }

        private bool IsValueParser(Type type)
        {
            return ValueParserType.IsAssignableFrom(type);
        }

        private bool IsCollectionOperand(ArgumentModel arg)
        {
            return arg.Kind == ArgumentKind.Operand
                && _collectionCtorProvider.IsCollection(arg.Type);
        }

        private string GetErrorMessage(ArgumentModel arg, string message)
        {
            return $"Invalid argument `{arg.Parameter.Name}`: {message}";
        }
    }
}
