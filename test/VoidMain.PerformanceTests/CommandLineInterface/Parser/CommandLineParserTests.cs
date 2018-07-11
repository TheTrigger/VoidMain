using BenchmarkDotNet.Attributes;
using VoidMain.CommandLineInterface.Parser;
using VoidMain.CommandLineInterface.Parser.Syntax;

namespace VoidMain.PerformanceTests.CommandLineInterface.Parser
{
    [MemoryDiagnoser]
    public class CommandLineParserTests
    {
        private const string Command_NameOnly = "command1 command2 command3";
        private const string Command_OptionsOnly = "--option1 value --option2 value --option3 value";
        private const string Command_OperandsOnly = "-- operand1 operand2 operand3";
        private const string Command_Short = "command --option value -- operand";
        private const string Command_Long = "command1 command2 command3 --option1 value --option2 value --option3 value -- operand1 operand2 operand3";

        private ICommandLineParser _parser;

        [GlobalSetup]
        public void Setup()
        {
            var syntaxFactory = new SyntaxFactory();
            var lexer = new CommandLineLexer(syntaxFactory);
            var semanticModel = new EmptySemanticModel();
            _parser = new CommandLineParser(lexer, semanticModel, syntaxFactory);
        }

        [Benchmark]
        public CommandLineSyntax CommandNameOnly()
        {
            return _parser.Parse(Command_NameOnly);
        }

        [Benchmark]
        public CommandLineSyntax OptionsOnly()
        {
            return _parser.Parse(Command_OptionsOnly);
        }

        [Benchmark]
        public CommandLineSyntax OperandsOnly()
        {
            return _parser.Parse(Command_OperandsOnly);
        }

        [Benchmark]
        public CommandLineSyntax ShortCommand()
        {
            return _parser.Parse(Command_Short);
        }

        [Benchmark]
        public CommandLineSyntax LongCommand()
        {
            return _parser.Parse(Command_Long);
        }
    }
}
