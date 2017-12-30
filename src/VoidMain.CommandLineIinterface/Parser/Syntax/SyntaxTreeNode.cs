using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VoidMain.CommandLineIinterface.Parser.Syntax
{
    public abstract class SyntaxTreeNode : SyntaxNode, IReadOnlyList<SyntaxNode>
    {
        private readonly SyntaxNode[] _children;

        public int Count => _children.Length;
        public SyntaxNode this[int index] => _children[index];

        protected SyntaxTreeNode(SyntaxKind kind, IEnumerable<SyntaxNode> children)
        {
            Kind = kind;
            _children = children.Where(_ => _ != null).ToArray();

            if (_children.Length == 0)
            {
                Span = TextSpan.Empty;
                FullSpan = TextSpan.Empty;
                return;
            }

            var first = _children[0];
            var last = _children[_children.Length - 1];

            Span = TextSpan.RangeInclusive(first.Span, last.Span);
            FullSpan = TextSpan.RangeInclusive(first.FullSpan, last.FullSpan);
        }

        protected abstract bool AcceptSelf<TParam>(ICommandLineSyntaxVisitor<TParam> visitor, TParam param);

        public void Accept<TParam>(ICommandLineSyntaxVisitor<TParam> visitor, TParam param)
        {
            if (AcceptSelf(visitor, param))
            {
                foreach (var child in this.OfType<SyntaxTreeNode>())
                {
                    child.Accept(visitor, param);
                }
            }
        }

        public void Accept<TParam>(ISyntaxTokenVisitor<TParam> visitor, TParam param)
        {
            if (visitor.VisitNode(this, param))
            {
                foreach (var child in this)
                {
                    if (child is SyntaxTreeNode treeNode)
                    {
                        treeNode.Accept(visitor, param);
                    }
                    else
                    {
                        visitor.VisitNode(child, param);
                    }
                }
            }
        }

        public IEnumerator<SyntaxNode> GetEnumerator() => ((IEnumerable<SyntaxNode>)_children).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _children.GetEnumerator();
    }
}
