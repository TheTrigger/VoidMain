namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public interface ISyntaxTokenVisitor<TParam>
    {
        bool VisitNode(SyntaxNode node, TParam param);
    }
}
