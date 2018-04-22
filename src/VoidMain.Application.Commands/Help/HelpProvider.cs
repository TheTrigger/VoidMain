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
        private readonly ICommandFormatter _commandFormatter;
        private readonly int ListOffset = 2;
        private readonly int ArgumentDescriptionOffset = 4;

        public HelpProvider(ICollectionConstructorProvider colCtorProvider)
        {
            _commandFormatter = new CommandHelpFormatter(colCtorProvider);
        }

        public string GetGeneralHelp(IEnumerable<CommandModel> commands)
        {
            var help = new StringBuilder();

            string usage = GetUsage();
            help.AppendLine(usage)
                .AppendLine()
                .AppendLine("Commands:");

            commands = commands.OrderBy(_ => _commandFormatter.FormatCommandName(_.Name));
            foreach (var command in commands)
            {
                string signature = _commandFormatter.FormatCommand(command);
                help.Append(' ', ListOffset)
                    .AppendLine(signature);
            }

            return help.ToString();
        }

        private string GetUsage()
        {
            return "command name [options...] [operands...]";
        }

        public string GetCommandHelp(CommandModel command)
        {
            var help = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(command.Description))
            {
                help.AppendLine(command.Description)
                    .AppendLine();
            }

            string signature = _commandFormatter.FormatCommand(command);
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
                .OrderBy(_ => _.Name)
                .ToArray();

            if (options.Length == 0) return;

            help.AppendLine()
                .AppendLine("Options:");

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
                    help.Append(' ', descrOffset)
                        .Append(opt.Description);
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

            help.AppendLine()
                .AppendLine("Operands:");

            foreach (var opr in operands)
            {
                help.Append(' ', ListOffset);
                int nameLength = opr.Name.Length;

                help.Append(opr.Name);

                if (!String.IsNullOrWhiteSpace(opr.Description))
                {
                    int descrOffset = nameSpace - nameLength + ArgumentDescriptionOffset;
                    help.Append(' ', descrOffset)
                        .Append(opr.Description);
                }

                help.AppendLine();
            }
        }
    }
}
