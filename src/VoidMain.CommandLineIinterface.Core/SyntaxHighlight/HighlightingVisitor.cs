using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class HighlightingVisitor<TStyle>
        : ICommandLineSyntaxVisitor<HighlightingVisitorParams<TStyle>>
    {
        private StyledSpan<TStyle> GetStyledSpan(
            SyntaxClass @class, TextSpan span,
            SyntaxHighlightingPalette<TStyle> palette)
        {
            palette.GetStyle(@class, out TStyle style);
            return new StyledSpan<TStyle>(span, style);
        }

        private void FillGapIfExists(
            int start, TextSpan span,
            SyntaxHighlightingPalette<TStyle> palette,
            List<StyledSpan<TStyle>> spans)
        {
            if (start < span.Start)
            {
                var gap = new TextSpan(span.Source, start, span.Start - start);
                spans.Add(new StyledSpan<TStyle>(gap, palette.DefaultStyle));
            }
        }

        public bool VisitCommandLine(CommandLineSyntax commandLine, HighlightingVisitorParams<TStyle> param)
        {
            param.HighlightedLength = 0;
            return true;
        }

        public bool VisitCommandName(CommandNameSyntax commandName, HighlightingVisitorParams<TStyle> param)
        {
            var spans = param.Spans;
            var palette = param.Palette;
            foreach (var namePart in commandName.NameParts)
            {
                FillGapIfExists(param.HighlightedLength, namePart.Span, palette, spans);
                spans.Add(GetStyledSpan(SyntaxClass.CommandName, namePart.Span, palette));
                param.HighlightedLength = namePart.Span.End;
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
            var palette = param.Palette;

            if (option.NameMarker != null)
            {
                FillGapIfExists(param.HighlightedLength, option.NameMarker.Span, palette, spans);
                spans.Add(GetStyledSpan(SyntaxClass.OptionNameMarker, option.NameMarker.Span, palette));
                param.HighlightedLength = option.NameMarker.Span.End;
            }

            FillGapIfExists(param.HighlightedLength, option.Name.Span, palette, spans);
            spans.Add(GetStyledSpan(SyntaxClass.OptionName, option.Name.Span, palette));
            param.HighlightedLength = option.Name.Span.End;

            if (option.ValueMarker != null)
            {
                FillGapIfExists(param.HighlightedLength, option.ValueMarker.Span, palette, spans);
                spans.Add(GetStyledSpan(SyntaxClass.OptionValueMarker, option.ValueMarker.Span, palette));
                param.HighlightedLength = option.ValueMarker.Span.End;
            }

            if (option.Value != null)
            {
                FillGapIfExists(param.HighlightedLength, option.Value.Span, palette, spans);
                spans.Add(GetStyledSpan(SyntaxClass.OptionValue, option.Value.Span, palette));
                param.HighlightedLength = option.Value.Span.End;
            }

            return false;
        }

        public bool VisitOperandsSectionMarker(OperandsSectionMarkerSyntax marker, HighlightingVisitorParams<TStyle> param)
        {
            FillGapIfExists(param.HighlightedLength, marker.Span, param.Palette, param.Spans);
            param.Spans.Add(GetStyledSpan(SyntaxClass.OperandsSectionMarker, marker.Span, param.Palette));
            param.HighlightedLength = marker.Span.End;
            return false;
        }

        public bool VisitOperand(OperandSyntax operand, HighlightingVisitorParams<TStyle> param)
        {
            FillGapIfExists(param.HighlightedLength, operand.Span, param.Palette, param.Spans);
            param.Spans.Add(GetStyledSpan(SyntaxClass.Operand, operand.Span, param.Palette));
            param.HighlightedLength = operand.Span.End;
            return false;
        }

        public bool VisitValue(ValueSyntax value, HighlightingVisitorParams<TStyle> param)
        {
            return false;
        }
    }
}
