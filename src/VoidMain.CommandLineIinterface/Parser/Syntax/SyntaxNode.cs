namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public abstract class SyntaxNode
    {
        public SyntaxKind Kind { get; protected set; }
        public TextSpan Span { get; protected set; }
        public virtual TextSpan FullSpan { get; protected set; }
        public bool IsMissing { get; protected set; }

        public virtual bool IsTrivia => false;
        public virtual bool IsToken => false;

        public override string ToString() => $"\"{Span.Text}\" [{Kind}]";
    }
}
