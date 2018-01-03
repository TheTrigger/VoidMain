using System;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface.Parser
{
    public interface ISemanticModel
    {
        /// <summary>
        /// Determines whether the subcommand with the specified name exists.
        /// </summary>
        /// <param name="commandName">Command name.</param>
        /// <param name="subcommand">Subcommand name.</param>
        /// <returns>Returns true if command exists.</returns>
        bool HasSubCommand(IReadOnlyList<string> commandName, string subcommand);

        /// <summary>
        /// Returns options's value type if existed.
        /// </summary>
        /// <param name="commandName">Command name.</param>
        /// <param name="optionName">Name of the option.</param>
        /// <param name="valueType">Value type of the option.</param>
        /// <returns>Returns true if command name have this option.</returns>
        bool TryGetOptionType(IReadOnlyList<string> commandName, string optionName, out Type valueType);

        /// <summary>
        /// Returns operand's value type if existed.
        /// </summary>
        /// <param name="commandName">Command name.</param>
        /// <param name="operandIndex">Zero-based index of the operand.</param>
        /// <param name="valueType">Value type of the operand.</param>
        /// <returns>Returns true if command name have this operand.</returns>
        bool TryGetOperandType(IReadOnlyList<string> commandName, int operandIndex, out Type valueType);
    }
}
