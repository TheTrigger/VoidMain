using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class HighlightingVisitor<TColor>
        : ICommandLineSyntaxVisitor<HighlightingVisitorParams<TColor>>
    {
        private ColoredSpan<TColor> GetColoredSpan(
            SyntaxClass @class, TextSpan span,
            SyntaxPallete<TColor> pallete)
        {
            pallete.GetColors(@class, out TColor background, out TColor foreground);
            return new ColoredSpan<TColor>(span, background, foreground);
        }

        public bool VisitCommandLine(CommandLineSyntax commandLine, HighlightingVisitorParams<TColor> param)
        {
            return true;
        }

        public bool VisitCommandName(CommandNameSyntax commandName, HighlightingVisitorParams<TColor> param)
        {
            var spans = param.Spans;
            var pallete = param.Pallete;
            foreach (var namePart in commandName.NameParts)
            {
                spans.Add(GetColoredSpan(SyntaxClass.CommandName, namePart.Span, pallete));
            }
            return false;
        }

        public bool VisitArgumentsSection(ArgumentsSectionSyntax argumentsSection, HighlightingVisitorParams<TColor> param)
        {
            return true;
        }

        public bool VisitOption(OptionSyntax option, HighlightingVisitorParams<TColor> param)
        {
            var spans = param.Spans;
            var pallete = param.Pallete;
            spans.Add(GetColoredSpan(SyntaxClass.OptionNameMarker, option.NameMarker.Span, pallete));
            spans.Add(GetColoredSpan(SyntaxClass.OptionName, option.Name.Span, pallete));
            if (option.ValueMarker != null)
            {
                spans.Add(GetColoredSpan(SyntaxClass.OptionValueMarker, option.ValueMarker.Span, pallete));
            }
            if (option.Value != null)
            {
                spans.Add(GetColoredSpan(SyntaxClass.OptionValue, option.Value.Span, pallete));
            }
            return false;
        }

        public bool VisitOperandsSectionMarker(OperandsSectionMarkerSyntax marker, HighlightingVisitorParams<TColor> param)
        {
            param.Spans.Add(GetColoredSpan(SyntaxClass.OperandsSectionMarker, marker.Span, param.Pallete));
            return false;
        }

        public bool VisitOperand(OperandSyntax operand, HighlightingVisitorParams<TColor> param)
        {
            param.Spans.Add(GetColoredSpan(SyntaxClass.Operand, operand.Span, param.Pallete));
            return false;
        }

        public bool VisitValue(ValueSyntax value, HighlightingVisitorParams<TColor> param)
        {
            return false;
        }
    }
}
