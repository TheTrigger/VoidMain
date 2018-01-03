using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Parser
{
    public class EmptySemanticModel : ISemanticModel
    {
        public bool HasSubCommand(IReadOnlyList<string> commandName, string subcommand)
        {
            return true;
        }

        public bool TryGetOptionType(
            IReadOnlyList<string> commandName, string optionName, out Type valueType)
        {
            valueType = typeof(string);
            return true;
        }

        public bool TryGetOperandType(
            IReadOnlyList<string> commandName, int operandIndex, out Type valueType)
        {
            valueType = typeof(string);
            return true;
        }
    }
}
