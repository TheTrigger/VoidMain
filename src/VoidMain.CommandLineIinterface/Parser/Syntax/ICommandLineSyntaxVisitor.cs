namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public interface ICommandLineSyntaxVisitor<in TParam>
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
        /// Visits VisitArgumentsSection node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitArgumentsSection(ArgumentsSectionSyntax arguments, TParam param);

        /// <summary>
        /// Visits OptionSyntax node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitOption(OptionSyntax option, TParam param);

        /// <summary>
        /// Visits OperandsSectionMarkerSyntax node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitOperandsSectionMarker(OperandsSectionMarkerSyntax marker, TParam param);

        /// <summary>
        /// Visits OperandSyntax node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitOperand(OperandSyntax operand, TParam param);

        /// <summary>
        /// Visits ValueSyntax node.
        /// </summary>
        /// <returns>True if need to visit children nodes.</returns>
        bool VisitValue(ValueSyntax value, TParam param);
    }
}
