namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public interface ISyntaxNodeVisitor<in TParam>
    {
        bool VisitNode(SyntaxNode node, TParam param);
    }
}
