using System.Collections.Generic;
using VoidMain.CommandLineIinterface.Parser.Syntax;

namespace VoidMain.CommandLineIinterface.Highlighting.CommandLine
{
    public class CommandLineHighlightingVisitor<TStyle>
        : ICommandLineSyntaxVisitor<CommandLineHighlightingVisitorParams<TStyle>>
    {
        public bool VisitCommandLine(CommandLineSyntax commandLine, CommandLineHighlightingVisitorParams<TStyle> param)
        {
            return true;
        }

        public bool VisitCommandName(CommandNameSyntax commandName, CommandLineHighlightingVisitorParams<TStyle> param)
        {
            var spans = param.Spans;
            var palette = param.Palette;

            foreach (var namePart in commandName.NameParts)
            {
                AddSpans(spans, namePart, CommandLineStyleName.CommandName, palette);
            }

            return false;
        }

        public bool VisitArgumentsSection(ArgumentsSectionSyntax argumentsSection, CommandLineHighlightingVisitorParams<TStyle> param)
        {
            return true;
        }

        public bool VisitOption(OptionSyntax option, CommandLineHighlightingVisitorParams<TStyle> param)
        {
            var spans = param.Spans;
            var palette = param.Palette;

            if (option.NameMarker != null)
            {
                AddSpans(spans, option.NameMarker, CommandLineStyleName.OptionNameMarker, palette);
            }

            AddSpans(spans, option.Name, CommandLineStyleName.OptionName, palette);

            if (option.ValueMarker != null)
            {
                AddSpans(spans, option.ValueMarker, CommandLineStyleName.OptionValueMarker, palette);
            }

            if (option.Value != null)
            {
                AddSpans(spans, option.Value, CommandLineStyleName.OptionValue, palette);
            }

            return false;
        }

        public bool VisitEndOfOptions(EndOfOptionsSyntax endOfOptions, CommandLineHighlightingVisitorParams<TStyle> param)
        {
            AddSpans(param.Spans, endOfOptions.Token, CommandLineStyleName.EndOfOptions, param.Palette);
            return false;
        }

        public bool VisitOperand(OperandSyntax operand, CommandLineHighlightingVisitorParams<TStyle> param)
        {
            AddSpans(param.Spans, operand.Value, CommandLineStyleName.Operand, param.Palette);
            return false;
        }

        public bool VisitValue(ValueSyntax value, CommandLineHighlightingVisitorParams<TStyle> param)
        {
            return false;
        }

        private void AddSpans(
            List<StyledSpan<TStyle>> spans,
            SyntaxToken token,
            CommandLineStyleName @class,
            HighlightingPalette<CommandLineStyleName, TStyle> palette)
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
            CommandLineStyleName @class,
            HighlightingPalette<CommandLineStyleName, TStyle> palette)
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
            CommandLineStyleName @class, TextSpan span,
            HighlightingPalette<CommandLineStyleName, TStyle> palette)
        {
            var style = palette.Get(@class);
            return new StyledSpan<TStyle>(span, style);
        }

        private StyledSpan<TStyle> GetDefaultSpan(
            TextSpan span,
            HighlightingPalette<CommandLineStyleName, TStyle> palette)
        {
            return new StyledSpan<TStyle>(span, palette.DefaultStyle);
        }
    }
}
