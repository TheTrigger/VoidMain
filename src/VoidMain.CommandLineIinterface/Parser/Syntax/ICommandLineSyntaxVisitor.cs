namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public interface ICommandLineSyntaxVisitor<TParam>
    {
        /// <summary>
        /// Visits CommandLineSyntax node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitCommandLine(CommandLineSyntax commandLine, TParam param);

        /// <summary>
        /// Visits CommandNameSyntax node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitCommandName(CommandNameSyntax commandName, TParam param);

        /// <summary>
        /// Visits OptionsSectionSyntax node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitOptionsSection(OptionsSectionSyntax optionsSection, TParam param);

        /// <summary>
        /// Visits OperandsSectionSyntax node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitOperandsSection(OperandsSectionSyntax operandsSection, TParam param);

        /// <summary>
        /// Visits OptionSyntax node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitOption(OptionSyntax option, TParam param);

        /// <summary>
        /// Visits OperandSyntax node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitOperand(OperandSyntax operand, TParam param);
    }
}
