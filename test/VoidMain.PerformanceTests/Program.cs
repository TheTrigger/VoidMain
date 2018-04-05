using BenchmarkDotNet.Running;
using VoidMain.PerformanceTests.CommandLineIinterface.Parser;

namespace VoidMain.PerformanceTests
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<CommandLineParserTests>();
        }
    }
}
