namespace VoidMain.Text.Templates.Parser
{
    public readonly struct ParseRange : IParseRange
    {
        private readonly char EndSymbol { get; }

        public ParseRange(char endSymbol)
        {
            EndSymbol = endSymbol;
        }

        public bool IsEndOfRange(string template, int position)
        {
            return template[position] == EndSymbol;
        }
    }
}
