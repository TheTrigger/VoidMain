using System;
using System.Text;
using VoidMain.Application.Commands.Arguments;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Help
{
    public class CommandHelpFormatter : ICommandFormatter
    {
        private readonly ICollectionConstructorProvider _colCtorProvider;

        public CommandHelpFormatter(ICollectionConstructorProvider colCtorProvider)
        {
            _colCtorProvider = colCtorProvider ?? throw new ArgumentNullException(nameof(colCtorProvider));
        }

        public string FormatCommand(CommandModel command)
        {
            var result = new StringBuilder();

            string commandName = FormatCommandName(command.Name);
            result.Append(commandName);

            foreach (var arg in command.Arguments)
            {
                string formatted = FormatArgument(arg);
                if (!String.IsNullOrEmpty(formatted))
                {
                    result.Append(' ').Append(formatted);
                }
            }

            return result.ToString();
        }

        public string FormatCommandName(CommandName commandName)
        {
            return String.Join(" ", commandName.Parts);
        }

        public string FormatArgument(ArgumentModel argument)
        {
            switch (argument.Kind)
            {
                case ArgumentKind.Option:
                    return FormatOption(argument);
                case ArgumentKind.Operand:
                    return FormatOperand(argument);
                default:
                    return String.Empty;
            }
        }

        private string FormatOption(ArgumentModel option)
        {
            string name = option.Name;

            if (_colCtorProvider.IsCollection(option.Type))
            {
                if (option.Optional)
                {
                    return $"[(--{name} <value>)...]";
                }
                else
                {
                    return $"(--{name} <value>)...";
                }
            }

            if (option.Optional)
            {
                return $"[--{name} <value>]";
            }
            else
            {
                return $"--{name} <value>";
            }
        }

        private string FormatOperand(ArgumentModel operand)
        {
            string name = operand.Name;

            if (_colCtorProvider.IsCollection(operand.Type))
            {
                if (operand.Optional)
                {
                    return $"[<{name}>...]";
                }
                else
                {
                    return $"<{name}>...";
                }
            }

            if (operand.Optional)
            {
                return $"[<{name}>]";
            }
            else
            {
                return $"<{name}>";
            }
        }
    }
}
