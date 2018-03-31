using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class HighlightingVisitor<TStyle>
        : ICommandLineSyntaxVisitor<HighlightingVisitorParams<TStyle>>
    {
        private StyledSpan<TStyle> GetStyledSpan(
            SyntaxClass @class, TextSpan span,
            SyntaxHighlightingPallete<TStyle> pallete)
        {
            pallete.GetStyle(@class, out TStyle style);
            return new StyledSpan<TStyle>(span, style);
        }

        public bool VisitCommandLine(CommandLineSyntax commandLine, HighlightingVisitorParams<TStyle> param)
        {
            return true;
        }

        public bool VisitCommandName(CommandNameSyntax commandName, HighlightingVisitorParams<TStyle> param)
        {
            var spans = param.Spans;
            var pallete = param.Pallete;
            foreach (var namePart in commandName.NameParts)
            {
                spans.Add(GetStyledSpan(SyntaxClass.CommandName, namePart.Span, pallete));
            }
            return false;
        }

        public bool VisitArgumentsSection(ArgumentsSectionSyntax argumentsSection, HighlightingVisitorParams<TStyle> param)
        {
            return true;
        }

        public bool VisitOption(OptionSyntax option, HighlightingVisitorParams<TStyle> param)
        {
            var spans = param.Spans;
            var pallete = param.Pallete;
            if (option.NameMarker != null)
            {
                spans.Add(GetStyledSpan(SyntaxClass.OptionNameMarker, option.NameMarker.Span, pallete));
            }
            spans.Add(GetStyledSpan(SyntaxClass.OptionName, option.Name.Span, pallete));
            if (option.ValueMarker != null)
            {
                spans.Add(GetStyledSpan(SyntaxClass.OptionValueMarker, option.ValueMarker.Span, pallete));
            }
            if (option.Value != null)
            {
                spans.Add(GetStyledSpan(SyntaxClass.OptionValue, option.Value.Span, pallete));
            }
            return false;
        }

        public bool VisitOperandsSectionMarker(OperandsSectionMarkerSyntax marker, HighlightingVisitorParams<TStyle> param)
        {
            param.Spans.Add(GetStyledSpan(SyntaxClass.OperandsSectionMarker, marker.Span, param.Pallete));
            return false;
        }

        public bool VisitOperand(OperandSyntax operand, HighlightingVisitorParams<TStyle> param)
        {
            param.Spans.Add(GetStyledSpan(SyntaxClass.Operand, operand.Span, param.Pallete));
            return false;
        }

        public bool VisitValue(ValueSyntax value, HighlightingVisitorParams<TStyle> param)
        {
            return false;
        }
    }
}
