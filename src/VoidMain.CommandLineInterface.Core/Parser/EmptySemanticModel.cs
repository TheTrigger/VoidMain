using System;
using System.Collections.Generic;
using VoidMain.Application;

namespace VoidMain.CommandLineInterface.Parser
{
    public class EmptySemanticModel : ISemanticModel
    {
        private readonly Type StringType = typeof(string);

        public bool HasSubCommand(IReadOnlyList<string> commandName, string subcommand)
        {
            return true;
        }

        public bool TryGetOptionType(
            IReadOnlyList<string> commandName, string optionName, out Type valueType)
        {
            valueType = StringType;
            return true;
        }

        public bool TryGetOperandType(
            IReadOnlyList<string> commandName, int operandIndex, out Type valueType)
        {
            valueType = StringType;
            return true;
        }
    }
}
