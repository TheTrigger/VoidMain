using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.SyntaxHighlight
{
    public class HighlightingVisitor<TStyle>
        : ICommandLineSyntaxVisitor<HighlightingVisitorParams<TStyle>>
    {
        public bool VisitCommandLine(CommandLineSyntax commandLine, HighlightingVisitorParams<TStyle> param)
        {
            return true;
        }

        public bool VisitCommandName(CommandNameSyntax commandName, HighlightingVisitorParams<TStyle> param)
        {
            var spans = param.Spans;
            var palette = param.Palette;

            foreach (var namePart in commandName.NameParts)
            {
                AddSpans(spans, namePart, SyntaxClass.CommandName, palette);
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
                AddSpans(spans, option.NameMarker, SyntaxClass.OptionNameMarker, palette);
            }

            AddSpans(spans, option.Name, SyntaxClass.OptionName, palette);

            if (option.ValueMarker != null)
            {
                AddSpans(spans, option.ValueMarker, SyntaxClass.OptionValueMarker, palette);
            }

            if (option.Value != null)
            {
                AddSpans(spans, option.Value, SyntaxClass.OptionValue, palette);
            }

            return false;
        }

        public bool VisitOperandsSectionMarker(OperandsSectionMarkerSyntax marker, HighlightingVisitorParams<TStyle> param)
        {
            AddSpans(param.Spans, marker.SectionMarker, SyntaxClass.OperandsSectionMarker, param.Palette);
            return false;
        }

        public bool VisitOperand(OperandSyntax operand, HighlightingVisitorParams<TStyle> param)
        {
            AddSpans(param.Spans, operand.Value, SyntaxClass.Operand, param.Palette);
            return false;
        }

        public bool VisitValue(ValueSyntax value, HighlightingVisitorParams<TStyle> param)
        {
            return false;
        }

        private void AddSpans(
            List<StyledSpan<TStyle>> spans,
            SyntaxToken token,
            SyntaxClass @class,
            SyntaxHighlightingPalette<TStyle> palette)
        {
            if (token.HasLeadingTrivia)
            {
                spans.Add(GetDefaultSpan(token.LeadingTrivia.Span, palette));
            }

            spans.Add(GetStyledSpan(@class, token.Span, palette));

            if (token.HasTrailingTrivia)
            {
                spans.Add(GetDefaultSpan(token.TrailingTrivia.Span, palette));
            }
        }

        private void AddSpans(
            List<StyledSpan<TStyle>> spans,
            ValueSyntax valueSyntax,
            SyntaxClass @class,
            SyntaxHighlightingPalette<TStyle> palette)
        {
            var valueTokens = valueSyntax.Tokens;
            var first = valueTokens[0];
            var last = valueTokens[valueTokens.Count - 1];

            if (first.HasLeadingTrivia)
            {
                spans.Add(GetDefaultSpan(first.LeadingTrivia.Span, palette));
            }

            spans.Add(GetStyledSpan(@class, valueSyntax.Span, palette));

            if (last.HasTrailingTrivia)
            {
                spans.Add(GetDefaultSpan(last.TrailingTrivia.Span, palette));
            }
        }

        private StyledSpan<TStyle> GetStyledSpan(
            SyntaxClass @class, TextSpan span,
            SyntaxHighlightingPalette<TStyle> palette)
        {
            var style = palette.GetStyle(@class);
            return new StyledSpan<TStyle>(span, style);
        }

        private StyledSpan<TStyle> GetDefaultSpan(
            TextSpan span,
            SyntaxHighlightingPalette<TStyle> palette)
        {
            return new StyledSpan<TStyle>(span, palette.DefaultStyle);
        }
    }
}
