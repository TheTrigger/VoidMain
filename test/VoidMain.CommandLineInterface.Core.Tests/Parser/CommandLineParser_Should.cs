using Moq;
using System;
using VoidMain.Application;
using VoidMain.CommandLineInterface.Parser.Syntax;
using Xunit;

namespace VoidMain.CommandLineInterface.Parser.Tests
{
    public class CommandLineParser_Should
    {
        #region Ctor tests

        [Fact]
        public void RequireLexer()
        {
            var semanticModel = new Mock<ISemanticModel>();
            var syntaxFactory = new Mock<SyntaxFactory>();
            Assert.Throws<ArgumentNullException>(() => new CommandLineParser(null, semanticModel.Object, syntaxFactory.Object));
        }

        [Fact]
        public void RequireSemanticModel()
        {
            var lexer = new Mock<ICommandLineLexer>();
            var syntaxFactory = new Mock<SyntaxFactory>();
            Assert.Throws<ArgumentNullException>(() => new CommandLineParser(lexer.Object, null, syntaxFactory.Object));
        }

        [Fact]
        public void RequireSyntaxFactory()
        {
            var lexer = new Mock<ICommandLineLexer>();
            var semanticModel = new Mock<ISemanticModel>();
            Assert.Throws<ArgumentNullException>(() => new CommandLineParser(lexer.Object, semanticModel.Object, null));
        }

        #endregion
    }
}
