using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoidMain.Application.Commands.Arguments;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Help
{
    public class HelpProvider
    {
        private readonly ICollectionConstructorProvider _colCtorProvider;
        private readonly int ListOffset = 2;
        private readonly int ArgumentDescriptionOffset = 4;

        public HelpProvider(ICollectionConstructorProvider colCtorProvider)
        {
            _colCtorProvider = colCtorProvider ?? throw new ArgumentNullException(nameof(colCtorProvider));
        }

        public string GetGeneralHelp(IEnumerable<CommandModel> commands)
        {
            var help = new StringBuilder();

            string usage = GetUsage();
            help.AppendLine(usage)
                .AppendLine()
                .AppendLine("Commands:");

            // TODO: use comparer
            commands = commands.OrderBy(_ => _.Name.ToString());
            foreach (var command in commands)
            {
                string signature = GetCommandSignature(command);
                help.Append(' ', ListOffset);
                help.AppendLine(signature);
            }

            return help.ToString();
        }

        public string GetCommandHelp(CommandModel command)
        {
            var help = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(command.Description))
            {
                help.AppendLine(command.Description);
                help.AppendLine();
            }

            string signature = GetCommandSignature(command);
            help.AppendLine(signature);

            var arguments = command.Arguments
                .Where(_ => _.Kind == ArgumentKind.Option
                         || _.Kind == ArgumentKind.Operand)
                .ToArray();

            if (arguments.Length > 0)
            {
                int maxNameLength = arguments.Max(_ => _.Name.Length + (_.Alias?.Length ?? 0));

                AppendOptionsList(help, command, maxNameLength);
                AppendOperandsList(help, command, maxNameLength);
            }

            return help.ToString();
        }

        private void AppendOptionsList(StringBuilder help, CommandModel command, int nameSpace)
        {
            var options = command.Arguments
                .Where(_ => _.Kind == ArgumentKind.Option)
                .OrderBy(_ => _.Name) // TODO: use comparer
                .ToArray();

            if (options.Length == 0) return;

            help.AppendLine();
            help.AppendLine("Options:");

            foreach (var opt in options)
            {
                help.Append(' ', ListOffset);
                int nameLength = opt.Name.Length;

                if (!String.IsNullOrWhiteSpace(opt.Alias))
                {
                    nameLength += opt.Alias.Length + 3;
                    help.Append($"-{opt.Alias}, ");
                }

                help.Append($"--{opt.Name}");

                if (!String.IsNullOrWhiteSpace(opt.Description))
                {
                    int descrOffset = nameSpace - nameLength + ArgumentDescriptionOffset;
                    help.Append(' ', descrOffset);
                    help.Append(opt.Description);
                }

                help.AppendLine();
            }
        }

        private void AppendOperandsList(StringBuilder help, CommandModel command, int nameSpace)
        {
            var operands = command.Arguments
                .Where(_ => _.Kind == ArgumentKind.Operand)
                .ToArray();

            if (operands.Length == 0) return;

            help.AppendLine();
            help.AppendLine("Operands:");

            foreach (var opr in operands)
            {
                help.Append(' ', ListOffset);
                int nameLength = opr.Name.Length;

                help.Append(opr.Name);

                if (!String.IsNullOrWhiteSpace(opr.Description))
                {
                    int descrOffset = nameSpace - nameLength + ArgumentDescriptionOffset;
                    help.Append(' ', descrOffset);
                    help.Append(opr.Description);
                }

                help.AppendLine();
            }
        }

        private string GetUsage()
        {
            return "command_name [options...] [operands...]";
        }

        private string GetCommandSignature(CommandModel command)
        {
            var signature = new StringBuilder();
            signature.Append(command.Name);

            foreach (var arg in command.Arguments)
            {
                switch (arg.Kind)
                {
                    case ArgumentKind.Option:
                        string option = GetOptionSignature(arg);
                        signature.Append(' ').Append(option);
                        break;
                    case ArgumentKind.Operand:
                        string operand = GetOperandSignature(arg);
                        signature.Append(' ').Append(operand);
                        break;
                    default:
                        break;
                }
            }

            return signature.ToString();
        }

        private string GetOptionSignature(ArgumentModel option)
        {
            string name = option.Name;
            string value = GetOptionValuePlaceholer(option);

            if (_colCtorProvider.IsCollection(option.Type))
            {
                if (option.Optional)
                {
                    return $"[--{name} {value}]...";
                }
                else
                {
                    return $"--{name} {value} [--{name} {value}]...";
                }
            }
            else
            {
                if (option.Optional)
                {
                    return $"[--{name} {value}]";
                }
                else
                {
                    return $"--{name} {value}";
                }
            }
        }

        private string GetOptionValuePlaceholer(ArgumentModel option)
        {
            return "<value>";
        }

        private string GetOperandSignature(ArgumentModel operand)
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
                    return $"<{name}> [<{name}>...]";
                }
            }
            else
            {
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
}
