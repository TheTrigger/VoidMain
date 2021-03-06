﻿namespace VoidMain.Text.Templates.Parser
{
    public interface IPlaceholderParser<TPlaceholder>
    {
        int Parse<TParseRange>(
            string template, int position,
            TParseRange range, out TPlaceholder placeholder)
            where TParseRange : struct, IParseRange;
    }
}
