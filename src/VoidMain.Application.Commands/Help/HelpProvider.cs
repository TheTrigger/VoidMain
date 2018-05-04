using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoidMain.Application.Commands.Arguments.CollectionConstructors;
using VoidMain.Application.Commands.Model;

namespace VoidMain.Application.Commands.Help
{
    public class HelpProvider
    {
        private readonly ICommandFormatter _commandFormatter;
        private readonly int ListOffset = 2;
        private readonly int DescriptionOffset = 4;
        private readonly int MaxNameLength = 30;

        public HelpProvider(ICollectionConstructorProvider colCtorProvider)
        {
            _commandFormatter = new CommandHelpFormatter(colCtorProvider);
        }

        public string GetGeneralHelp(IReadOnlyList<CommandModel> commands)
        {
            var help = new StringBuilder();

            help.AppendLine("Usage:")
                .Append(' ', ListOffset)
                .AppendLine("command name [options...] [operands...]");

            if (commands.Count != 0)
            {
                help.AppendLine()
                    .AppendLine("Commands:");

                var commandsInfo = commands.Select(_ => new
                {
                    Name = _commandFormatter.FormatCommandName(_.Name),
                    _.Description
                })
                .OrderBy(_ => _.Name)
                .ToArray();

                int maxNameLength = Math.Min(MaxNameLength, commandsInfo.Max(_ => _.Name.Length));

                foreach (var cmdInfo in commandsInfo)
                {
                    help.Append(' ', ListOffset)
                        .Append(cmdInfo.Name);

                    if (!String.IsNullOrWhiteSpace(cmdInfo.Description))
                    {
                        int descrOffset = Math.Max(0, maxNameLength - cmdInfo.Name.Length) + DescriptionOffset;
                        help.Append(' ', descrOffset)
                            .Append(cmdInfo.Description);
                    }

                    help.AppendLine();
                }
            }

            return help.ToString();
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
            help.AppendLine("Syntax:")
                .Append(' ', ListOffset)
                .AppendLine(signature);

            var arguments = command.Arguments
                .Where(_ => _.Kind == ArgumentKind.Option
                         || _.Kind == ArgumentKind.Operand)
                .ToArray();

            if (arguments.Length > 0)
            {
                int maxNameLength = Math.Min(MaxNameLength, arguments.Max(_ => _.Name.Length + (_.Alias?.Length ?? 0)));

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
                int nameLength = 0;

                if (!String.IsNullOrWhiteSpace(opt.Alias))
                {
                    nameLength += opt.Alias.Length + 3;
                    help.Append($"-{opt.Alias}, ");
                }

                nameLength += opt.Name.Length + 2;
                help.Append($"--{opt.Name}");

                if (!String.IsNullOrWhiteSpace(opt.Description))
                {
                    int descrOffset = Math.Max(0, nameSpace - nameLength) + DescriptionOffset;
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
                    int descrOffset = Math.Max(0, nameSpace - nameLength) + DescriptionOffset;
                    help.Append(' ', descrOffset)
                        .Append(opr.Description);
                }

                help.AppendLine();
            }
        }
    }
}
