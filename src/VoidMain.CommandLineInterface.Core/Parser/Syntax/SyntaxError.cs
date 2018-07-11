namespace VoidMain.CommandLineInterface.Parser.Syntax
{
    public sealed class SyntaxError
    {
        public TextSpan Span { get; }
        public string Code { get; }
        public string Message { get; }

        public SyntaxError(TextSpan span, string code, string message)
        {
            Span = span;
            Code = code;
            Message = message;
        }

        public override string ToString() => $"{Code}: {Message}";
    }
}
