namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public interface ISyntaxNodeVisitor<TParam>
    {
        bool VisitNode(SyntaxNode node, TParam param);
    }
}
