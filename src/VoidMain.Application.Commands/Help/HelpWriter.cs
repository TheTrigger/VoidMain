using System;
using System.Collections.Generic;
using System.Linq;
using VoidMain.Application.Commands.Arguments.CollectionConstructors;
using VoidMain.Application.Commands.Model;
using VoidMain.CommandLineIinterface;

namespace VoidMain.Application.Commands.Help
{
    public class HelpWriter
    {
        private readonly int NestedLineOffset = 2;
        private readonly int DescriptionOffset = 4;
        private readonly int MaxNameLength = 30;
        private readonly string OptionNameAliasSeparator = ", ";

        private readonly ICommandFormatter _commandFormatter;

        public HelpWriter(ICollectionConstructorProvider colCtorProvider)
        {
            _commandFormatter = new CommandHelpFormatter(colCtorProvider);
        }

        #region General Help

        public void WriteGeneralHelp(ICommandLineOutput output, IReadOnlyList<CommandModel> commands)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            PrintUsage(output);
            output.WriteLine();

            if (commands.Count != 0)
            {
                PrintCommandsList(output, commands);
                output.WriteLine();
            }
        }

        private void PrintUsage(ICommandLineOutput output)
        {
            output.WriteLine(Color.White, "USAGE:");
            output.Write(' ', NestedLineOffset);
            output.WriteLine(new ColoredFormat("{0} [{1}] [{2}]")
            {
                { "command name", Color.Yellow},
                { "options...", Color.DarkGreen },
                { "operands...", Color.DarkCyan }
            });
        }

        private void PrintCommandsList(ICommandLineOutput output, IReadOnlyList<CommandModel> commands)
        {
            var commandsInfo = commands.Select(c => new
            {
                Name = _commandFormatter.FormatCommandName(c.Name),
                Command = c
            })
            .OrderBy(_ => _.Name)
            .ToArray();

            int maxNameLength = GetMax(commandsInfo, _ => _.Name.Length, MaxNameLength);

            output.WriteLine(Color.White, "COMMANDS:");

            foreach (var cmdInfo in commandsInfo)
            {
                output.Write(' ', NestedLineOffset);
                output.Write(Color.Yellow, cmdInfo.Name);

                if (!String.IsNullOrWhiteSpace(cmdInfo.Command.Description))
                {
                    int descrOffset = GetPadding(cmdInfo.Name.Length, maxNameLength) + DescriptionOffset;
                    output.Write(' ', descrOffset);
                    output.Write(Color.DarkGray, cmdInfo.Command.Description);
                }

                output.WriteLine();
            }
        }

        #endregion

        #region Command Help

        public void WriteCommandHelp(ICommandLineOutput output, CommandModel command)
        {
            string commandName = _commandFormatter.FormatCommandName(command.Name);
            output.WriteLine(Color.White, "NAME:");
            output.Write(' ', NestedLineOffset);
            output.WriteLine(Color.Yellow, commandName);
            output.WriteLine();

            if (!String.IsNullOrWhiteSpace(command.Description))
            {
                output.WriteLine(Color.White, "DESCRIPTION:");
                output.Write(' ', NestedLineOffset);
                output.WriteLine(Color.DarkGray, command.Description);
                output.WriteLine();
            }

            string syntax = _commandFormatter.FormatCommand(command);
            output.WriteLine(Color.White, "SYNTAX:");
            output.Write(' ', NestedLineOffset);
            output.WriteLine(syntax);
            output.WriteLine();

            var options = command.Arguments
                .Where(_ => _.Kind == ArgumentKind.Option)
                .OrderBy(_ => _.Name)
                .ToArray();

            var operands = command.Arguments
                .Where(_ => _.Kind == ArgumentKind.Operand)
                .ToArray();

            int maxOptionNameLength = GetMax(options, GetOptionNameLength, MaxNameLength);
            int maxOperandNameLength = GetMax(operands, _ => _.Name.Length, MaxNameLength);
            int maxArgNameLength = Math.Max(maxOptionNameLength, maxOperandNameLength);

            if (options.Length > 0)
            {
                PrintOptionsList(output, options, maxArgNameLength);
                output.WriteLine();
            }

            if (operands.Length > 0)
            {
                PrintOperandsList(output, operands, maxArgNameLength);
                output.WriteLine();
            }
        }

        private void PrintOptionsList(ICommandLineOutput output, ArgumentModel[] options, int nameSpace)
        {
            output.WriteLine(Color.White, "OPTIONS:");

            foreach (var opt in options)
            {
                output.Write(' ', NestedLineOffset);
                int nameLength = 0;

                if (!String.IsNullOrWhiteSpace(opt.Alias))
                {
                    output.Write(Color.DarkGreen, '-');
                    output.Write(Color.DarkGreen, opt.Alias);
                    output.Write(Color.DarkGreen, OptionNameAliasSeparator);
                    nameLength += 1 + opt.Alias.Length + OptionNameAliasSeparator.Length;
                }

                output.Write(Color.DarkGreen, "--");
                output.Write(Color.DarkGreen, opt.Name);
                nameLength += 2 + opt.Name.Length;

                if (!String.IsNullOrWhiteSpace(opt.Description))
                {
                    int descrOffset = GetPadding(nameLength, nameSpace) + DescriptionOffset;
                    output.Write(' ', descrOffset);
                    output.Write(Color.DarkGray, opt.Description);
                }

                output.WriteLine();
            }
        }

        private void PrintOperandsList(ICommandLineOutput output, ArgumentModel[] operands, int nameSpace)
        {
            output.WriteLine(Color.White, "OPERANDS:");

            foreach (var opr in operands)
            {
                output.Write(' ', NestedLineOffset);
                int nameLength = opr.Name.Length;

                output.Write(Color.DarkCyan, opr.Name);

                if (!String.IsNullOrWhiteSpace(opr.Description))
                {
                    int descrOffset = GetPadding(nameLength, nameSpace) + DescriptionOffset;
                    output.Write(' ', descrOffset);
                    output.Write(Color.DarkGray, opr.Description);
                }

                output.WriteLine();
            }
        }

        #endregion

        private int GetOptionNameLength(ArgumentModel option)
        {
            int length = 2 + option.Name.Length;
            if (option.Alias != null)
            {
                length += 1 + option.Alias.Length + OptionNameAliasSeparator.Length;
            }
            return length;
        }

        private static int GetMax<T>(T[] array, Func<T, int> getValue, int max)
        {
            if (array.Length == 0)
            {
                return 0;
            }
            return Math.Min(array.Max(getValue), max);
        }

        private static int GetPadding(int valueLength, int maxLength)
        {
            return Math.Max(0, maxLength - valueLength);
        }
    }
}
